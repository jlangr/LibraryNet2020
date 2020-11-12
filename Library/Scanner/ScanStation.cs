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
        private readonly HoldingsService holdingsService;
        private readonly PatronsService patronsService;

        public ScanStation(LibraryContext context, int branchId, IClassificationService classificationService,
            HoldingsService holdingsService, PatronsService patronsService)
        {
            this.classificationService = classificationService;
            this.holdingsService = holdingsService;
            this.patronsService = patronsService;
            BranchId = branchId;
            scannerBranchId = BranchId;
        }

        public void AddNewHolding(string isbn)
        {
            var classification = classificationService.Classification(isbn);
            holdingsService.Add(new Holding
            {
                Classification = classification,
                CopyNumber = holdingsService.NextAvailableCopyNumber(classification),
                BranchId = BranchId
            });
        }

        public void CompleteCheckout()
        {
            currentPatron = NoPatron;
        }

        public int BranchId { get; }

        public int CurrentPatronId => currentPatron;

        private bool InCheckinMode() => currentPatron == NoPatron;

        public void AcceptLibraryCard(int patronId)
        {
            currentPatron = patronId;
        }

        public void AcceptBarcode(string barcode)
        {
            var holding = holdingsService.FindByBarcode(barcode);
            if (InCheckinMode())
                HandleCheckin(holding, TimeService.Now);
            else
                HandleCheckout(holding, TimeService.Now);
        }

        private void HandleCheckout(Holding holding, DateTime timestamp)
        {
            if (holding.IsCheckedOut && IsCurrentPatronSameAsPatronWithHolding(holding))
            {
                CheckIn(holding, timestamp);
                CheckOut(holding, timestamp, CheckoutPolicies.BookCheckoutPolicy);
            }
            else
                CheckOut(holding, timestamp, CheckoutPolicies.BookCheckoutPolicy);
        }

        private void HandleCheckin(Holding holding, DateTime timestamp)
        {
            if (holding.IsCheckedOut)
                CheckIn(holding, timestamp);
            else
                throw new CheckoutException();
        }

        private void AssessLateReturnFine(Holding holding, DateTime timestamp)
        {
            var patron = patronsService.FindById(holding.HeldByPatronId);
            patron.Fine(FineAmount(holding, timestamp));
            patronsService.Update(patron);
        }

        private void CheckIn(Holding holding, DateTime timestamp)
        {
            AssessLateReturnFine(holding, timestamp);
            holding.CheckIn(timestamp, scannerBranchId);
            holdingsService.Update(holding);
        }

        private void CheckOut(Holding holding, DateTime timestamp, CheckoutPolicy bookCheckoutPolicy)
        {
            holding.CheckOut(timestamp, currentPatron, bookCheckoutPolicy);
            holdingsService.Update(holding);
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
    }
}