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
            this.branchId = BranchId;
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
        public void AcceptBarcode(string barCode)
        {
            var holdingService = holdingsService.FindByBarcode(barCode);
            var now = TimeService.Now;
            decimal fineAmount = GetFineAmount(holdingService, now);

            if (holdingService.IsCheckedOut && IsCurrentPatron())
            {
                CheckInAndFinePatron(holdingService, now, fineAmount);
            }
            else if (holdingService.IsCheckedOut)
            {
                CheckInAndFinePatron(holdingService, now, fineAmount);
                CheckoutAndUpdate(holdingService);
            }
            else if (!IsCurrentPatron())
            {
                CheckoutAndUpdate(holdingService);
            }
            else
            {
                throw new CheckoutException();
            }
        }

        private decimal GetFineAmount(Holding holdingService, DateTime now)
        {
            Material material = classificationService.Retrieve(holdingService.Classification);
            var fineAmount = material.CheckoutPolicy.FineAmount(holdingService.CheckOutTimestamp.Value, now);
            return fineAmount;
        }

        private void CheckoutAndUpdate(Holding holdingService)
        {
            holdingService.CheckOut(cts, currentPatron, CheckoutPolicies.BookCheckoutPolicy);
            holdingsService.Update(holdingService);
        }

        private bool IsCurrentPatron()
        {
            return currentPatron == NoPatron;
        }

        private void CheckInAndFinePatron(Holding holdingService, DateTime now, decimal fineAmount)
        {
            var patron = patronsService.FindById(holdingService.HeldByPatronId);
            patron.Fine(fineAmount);
            patronsService.Update(patron);
            holdingService.CheckIn(now, branchId);
            holdingsService.Update(holdingService);
        }

        public void CompleteCheckout()
        {
            currentPatron = NoPatron;
        }
    }
}