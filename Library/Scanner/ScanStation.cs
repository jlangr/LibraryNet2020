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
        private readonly int scannerBranchId;
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
            this.scannerBranchId = BranchId;
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

        public int BranchId { get; }

        public int CurrentPatronId => currentPatron;

        public void AcceptLibraryCard(int patronId)
        {
            currentPatron = patronId;
        }

        public void AcceptBarcode(string barcode)
        {
            var holding = holdingsService.FindByBarcode(barcode);

            var timestamp = TimeService.Now;
            if (InCheckinMode() && holding.IsCheckedOut)
            {
                AssessLateReturnFine(holding, timestamp);
                CheckIn(holding, timestamp);
            }
            else if (InCheckoutMode() && IsCurrentPatronSameAsPatronWithHolding(holding) && holding.IsCheckedOut)
            {
                AssessLateReturnFine(holding, timestamp);
                CheckIn(holding, timestamp);
                CheckOut(holding, timestamp, CheckoutPolicies.BookCheckoutPolicy);
            }
            else if (InCheckoutMode() && !holding.IsCheckedOut)
            {
                CheckOut(holding, timestamp, CheckoutPolicies.BookCheckoutPolicy);
            }
            else if (InCheckinMode() && !holding.IsCheckedOut)
                throw new CheckoutException();
        }

        private bool InCheckinMode()
        {
            return currentPatron == NoPatron;
        }

        private void AssessLateReturnFine(Holding holding, DateTime timestamp)
        {
            var patron = patronsService.FindById(holding.HeldByPatronId);
            patron.Fine(FineAmount(holding, timestamp));
            patronsService.Update(patron);
        }

        private void CheckIn(Holding holding, DateTime timestamp)
        {
            holding.CheckIn(timestamp, scannerBranchId);
            holdingsService.Update(holding);
        }

        private void CheckOut(Holding holding, DateTime timestamp, CheckoutPolicy bookCheckoutPolicy)
        {
            holding.CheckOut(timestamp, currentPatron, bookCheckoutPolicy);
            holdingsService.Update(holding);
        }

        private bool InCheckoutMode()
        {
            return currentPatron != NoPatron;
        }

        private decimal FineAmount(Holding holding, DateTime timestamp)
        {
            var material = classificationService.Retrieve(holding.Classification);
            return material.CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, timestamp);
        }

        private bool IsCurrentPatronSameAsPatronWithHolding(Holding h)
        {
            return h.HeldByPatronId != currentPatron;
        }

        public void CompleteCheckout()
        {
            currentPatron = NoPatron;
        }
    }
}