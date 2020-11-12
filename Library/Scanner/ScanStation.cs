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
        private int currentPatronId = NoPatron;
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

        public int CurrentPatronId => currentPatronId;

        public void AcceptLibraryCard(int patronId)
        {
            currentPatronId = patronId;
            cts = TimeService.Now;
        }

        public void AcceptBarcode(string barCode)
        {
            var holding = holdingsService.FindByBarcode(barCode);

            if (holding.IsCheckedOut)
            {
                if (currentPatronId == NoPatron)
                {
                    var patronId = holding.HeldByPatronId;
                    var currentDateAndTime = TimeService.Now;
                    Material material = classificationService.Retrieve(holding.Classification);

                    DetermineFine(holding, patronId, currentDateAndTime, material);

                    holding.CheckIn(currentDateAndTime, brId);
                    holdingsService.Update(holding);
                }
                else
                {
                    if (holding.HeldByPatronId != currentPatronId) // check out book already cked-out
                    {
                        var checkInDate = TimeService.Now;
                        var fine = classificationService.Retrieve(holding.Classification).CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, checkInDate);
                        var patron = patronsService.FindById(holding.HeldByPatronId);
                        patron.Fine(fine);
                        patronsService.Update(patron);
                        holding.CheckIn(checkInDate, brId);
                        holding.CheckOut(checkInDate, currentPatronId, CheckoutPolicies.BookCheckoutPolicy);
                        holdingsService.Update(holding);
                  
                    }
                    else // not checking out book already cked out by other patron
                    {
                        // otherwise ignore, already checked out by this patron
                    }
                }
            }
            else
            {
                if (currentPatronId != NoPatron) // check in book
                {
                    holding.CheckOut(cts, currentPatronId, CheckoutPolicies.BookCheckoutPolicy);
                    holdingsService.Update(holding);
                }
                else
                    throw new CheckoutException();
            }
        }

        private void DetermineFine(Holding holding, int patronId, DateTime currentDateAndTime, Material material)
        {
            var fine = material.CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, currentDateAndTime);
            Patron patron = patronsService.FindById(patronId);
            patron.Fine(fine);
            patronsService.Update(patron);
        }

        public void CompleteCheckout()
        {
            currentPatronId = NoPatron;
        }
    }
}