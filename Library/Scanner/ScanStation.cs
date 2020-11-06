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
        private readonly int branchId;
        private int currentPatronId = NoPatron;
        private DateTime currentTimeStamp;
        private readonly HoldingsService holdingsService;
        private readonly PatronsService patronsService;

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
            this.branchId = BranchId;
        }

        public void AddNewHolding(string isbn)
        {
            var classification = classificationService.Classification(isbn);
            var holding = new Holding
            {
                Classification = classification,
                CopyNumber = holdingsService.NextAvailableCopyNumber(classification),
                BranchId = BranchId
            };
            holdingsService.Add(holding);
        }

        public int BranchId { get; }

        public int CurrentPatronId => currentPatronId;

        public void AcceptLibraryCard(int patronId)
        {
            currentPatronId = patronId;
            currentTimeStamp = TimeService.Now;
        }

        public void AcceptBarcode(string barcode)
        {
            var holding = holdingsService.FindByBarcode(barcode);
            var timestamp = TimeService.Now;
            if (ScannerInCheckInState())
                if (holding.IsCheckedOut)
                {
                    CheckIn(holding, timestamp);
                }
                else
                {
                    ThrowDueToDuplicateCheckoutAttempt();
                }
            else // ScannerInCheckOutState
            {
                if (holding.IsAvailable)
                {
                    CheckOut(holding, currentTimeStamp);
                }
                else if (AlreadyCheckedOutByOtherPatron(holding))
                {
                    CheckIn(holding, timestamp);
                    CheckOut(holding, timestamp);
                }
            }
        }

        private void CheckIn(Holding holding, DateTime timestamp)
        {
            AssessFinesIfLateCheckin(holding, timestamp);
            holding.CheckIn(timestamp, branchId);
            holdingsService.Update(holding);
        }

        private void CheckOut(Holding holding, DateTime timeStamp)
        {
            holding.CheckOut(timeStamp, currentPatronId, CheckoutPolicies.BookCheckoutPolicy);
            holdingsService.Update(holding);
        }

        private static void ThrowDueToDuplicateCheckoutAttempt()
        {
            throw new CheckoutException();
        }

        private bool AlreadyCheckedOutByOtherPatron(Holding holding)
        {
            return holding.HeldByPatronId != currentPatronId;
        }

        private void AssessFinesIfLateCheckin(Holding holding, DateTime timestamp)
        {
            var material = classificationService.Retrieve(holding.Classification);
            var fine = material.CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, timestamp);
            var patron = patronsService.FindById(holding.HeldByPatronId);
            patron.Fine(fine);
            patronsService.Update(patron);
        }

        private bool ScannerInCheckInState()
        {
            return currentPatronId == NoPatron;
        }

        private void CheckOutStateHoldingNotCheckedOut(Holding holding, DateTime timestamp)
        {
            holding.CheckOut(timestamp, currentPatronId, CheckoutPolicies.BookCheckoutPolicy);
            holdingsService.Update(holding);
        }

        public void CompleteCheckout()
        {
            currentPatronId = NoPatron;
        }
    }
}