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
        public const int NoPatron = -1;
        private readonly IClassificationService classificationService;
        private readonly int brId;
        private int currentPatron = NoPatron;
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
            brId = BranchId;
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

        public int BranchId { get; private set; }

        public int CurrentPatronId => currentPatron;

        public void AcceptLibraryCard(int patronId)
        {
            currentPatron = patronId;
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
                if (currentPatron == NoPatron)
                {
                    var checkInTime = TimeService.Now;
                    Material material = classificationService.Retrieve(holding.Classification);
                    var fineAmount = material.CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, checkInTime);
                    FineHoldingPatron(holding, fineAmount);

                    holding.CheckIn(checkInTime, brId);
                    holdingsService.Update(holding);
                }
                else
                {
                    if (holding.HeldByPatronId != currentPatron) // check out book already cked-out
                    {
                        var n = TimeService.Now;
                        var f = classificationService.Retrieve(holding.Classification).CheckoutPolicy
                            .FineAmount(holding.CheckOutTimestamp.Value, n);
                        var patron = patronsService.FindById(holding.HeldByPatronId);
                        patron.Fine(f);
                        patronsService.Update(patron);
                        holding.CheckIn(n, brId);
                        holdingsService.Update(holding);
                        holding.CheckOut(n, currentPatron, CheckoutPolicies.BookCheckoutPolicy);
                        holdingsService.Update(holding);
                        // call check out controller(cur, bc1);
                    }
                }
            }
            else
            {
                if (currentPatron != NoPatron) // check in book
                {
                    holding.CheckOut(cts, currentPatron, CheckoutPolicies.BookCheckoutPolicy);
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
            currentPatron = NoPatron;
        }
    }
}