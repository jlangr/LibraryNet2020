using System;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using LibraryNet2020.Services;
using LibraryNet2020.Util;

namespace LibraryNet2020.Scanner
{
    public class ScanStation
    {
        public int BranchId { get; private set; }
        public int CurrentPatronId { get; set; } = NoPatron;
        public const int NoPatron = -1;
        private readonly IClassificationService classificationService;
        private DateTime cts;
        private HoldingsService holdingsService;
        private PatronsService patronsService;

        public ScanStation(LibraryContext context, int branchId)
            : this(context,
                branchId,
                new MasterClassificationService(),
                new HoldingsService(context),
                new PatronsService(context))
        {
        }

        public ScanStation(LibraryContext _context, int branchId, IClassificationService classificationService,
            HoldingsService holdingsService, PatronsService patronsService)
        {
            this.classificationService = classificationService;
            this.holdingsService = holdingsService;
            this.patronsService = patronsService;
            BranchId = branchId;
        }

        public Holding AddNewHolding(string isbn)
        {
            var classification = classificationService.Classification(isbn);
            var holding = new Holding
            {
                Classification = classification,
                CopyNumber = holdingsService.NextAvailableCopyNumber(classification),
                BranchId = BranchId
            };
            holdingsService.Add(holding);
            return holding;
        }

        public void AcceptLibraryCard(int patronId)
        {
            CurrentPatronId = patronId;
            cts = TimeService.Now;
        }

        // 1/19/2017: who wrote this?
        // 
        // FIXME. Fix this mess. We just have to SHIP IT for nwo!!!
        public void AcceptBarcode(string barcode)
        {
            var holding = holdingsService.FindByBarcode(barcode);

            if (holding.IsCheckedOut)
            {
                if (CurrentPatronId == NoPatron)
                {
                    var checkInTime = TimeService.Now;
                    Material material = classificationService.Retrieve(holding.Classification);
                    var fineAmount = material.CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, checkInTime);
                    FineHoldingPatron(holding, fineAmount);

                    holding.CheckIn(checkInTime, BranchId);
                    holdingsService.Update(holding);
                }
                else
                {
                    if (holding.HeldByPatronId != CurrentPatronId) // check out book already cked-out
                    {
                        var n = TimeService.Now;
                        var f = classificationService.Retrieve(holding.Classification).CheckoutPolicy
                            .FineAmount(holding.CheckOutTimestamp.Value, n);
                        var patron = patronsService.FindById(holding.HeldByPatronId);
                        patron.Fine(f);
                        patronsService.Update(patron);
                        holding.CheckIn(n, BranchId);
                        holdingsService.Update(holding);
                        holding.CheckOut(n, CurrentPatronId, CheckoutPolicies.BookCheckoutPolicy);
                        holdingsService.Update(holding);
                        // call check out controller(cur, bc1);
                    }
                }
            }
            else
            {
                if (CurrentPatronId != NoPatron) // check in book
                {
                    holding.CheckOut(cts, CurrentPatronId, CheckoutPolicies.BookCheckoutPolicy);
                    holdingsService.Update(holding);
                }
                else
                    throw new CheckoutException();
            }
        }

        private void FineHoldingPatron(Holding holding, decimal fineAmount)
        {
            Patron heldByPatron = patronsService.FindById(holding.HeldByPatronId);
            heldByPatron.Fine(fineAmount);
            patronsService.Update(heldByPatron);
        }

        public void CompleteCheckout()
        {
            CurrentPatronId = NoPatron;
        }
    }
}