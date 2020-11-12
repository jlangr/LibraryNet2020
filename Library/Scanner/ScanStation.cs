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
        private DateTime checkOutTimestamp;
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
            checkOutTimestamp = TimeService.Now;
        }

        // 1/19/2017: who wrote this?
        // 
        // FIXME. Fix this mess. We just have to SHIP IT for nwo!!!
        public void AcceptBarcode(string barcode)
        {
            var holding = holdingsService.FindByBarcode(barcode);
            if (IsCheckInScan(holding))
            {
                CheckInMaterial(holding, TimeService.Now);
            }
            else if (IsExchangeScan(holding))
            {
                ExchangeMaterial(holding);
            }
            else if (IsCheckOutScan(holding))
            {
                CheckOutMaterial(holding, checkOutTimestamp);
            }
            else
            {
                throw new CheckoutException();
            }
        }

        private bool IsCheckOutScan(Holding holding)
        {
            return !holding.IsCheckedOut && CurrentPatronId != NoPatron;
        }

        private bool IsExchangeScan(Holding holding)
        {
            return holding.IsCheckedOut && CurrentPatronId != NoPatron;
        }

        private bool IsCheckInScan(Holding holding)
        {
            return holding.IsCheckedOut && CurrentPatronId == NoPatron;
        }

        private void ExchangeMaterial(Holding holding)
        {
            if (holding.HeldByPatronId != CurrentPatronId) // check out book already cked-out
            {
                var exchangeTime = TimeService.Now;
                CheckInMaterial(holding, exchangeTime);
                CheckOutMaterial(holding, exchangeTime);
            }
        }

        private void CheckOutMaterial(Holding holding, DateTime checkInTime)
        {
            holding.CheckOut(checkInTime, CurrentPatronId, CheckoutPolicies.BookCheckoutPolicy);
            holdingsService.Update(holding);
        }

        private void CheckInMaterial(Holding holding, DateTime checkInTime)
        {
            FineHoldingPatron(holding, checkInTime);
            holding.CheckIn(checkInTime, BranchId);
            holdingsService.Update(holding);
        }

        private void FineHoldingPatron(Holding holding, DateTime checkInTime)
        {
            Patron heldByPatron = patronsService.FindById(holding.HeldByPatronId);
            Material material = classificationService.Retrieve(holding.Classification);
            var fineAmount = material.CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, checkInTime);
            heldByPatron.Fine(fineAmount);
            patronsService.Update(heldByPatron);
        }

        public void CompleteCheckout()
        {
            CurrentPatronId = NoPatron;
        }
    }
}