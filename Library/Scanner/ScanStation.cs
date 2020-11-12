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

            if (holdingService.IsCheckedOut)
            {
                if (currentPatron == NoPatron)
                {
                    // ci                  
                    var patronId = holdingService.HeldByPatronId;
                    var now = TimeService.Now;
                    Material material = classificationService.Retrieve(holdingService.Classification);                     
                    var fineAmount = material.CheckoutPolicy.FineAmount(holdingService.CheckOutTimestamp.Value, now);
                    
                    Patron patron = patronsService.FindById(patronId);
                    patron.Fine(fineAmount);
                    patronsService.Update(patron);
                    holdingService.CheckIn(now, branchId);
                    holdingsService.Update(holdingService);
                }
                else
                {
                    if (holdingService.HeldByPatronId != currentPatron) // check out book already cked-out
                    {
                      
                        var now = TimeService.Now;
                        var dueDate = TimeService.Now.AddDays(21);
                        var fineAmount = classificationService.Retrieve(holdingService.Classification).CheckoutPolicy
                            .FineAmount(holdingService.CheckOutTimestamp.Value, now);
                        
                        var patron = patronsService.FindById(holdingService.HeldByPatronId);                        
                        patron.Fine(fineAmount);
                        patronsService.Update(patron);
                        holdingService.CheckIn(now, branchId);
                        holdingsService.Update(holdingService);
                        // co
                        holdingService.CheckOut(now, currentPatron, CheckoutPolicies.BookCheckoutPolicy);
                        holdingsService.Update(holdingService);
                        // call check out controller(cur, barCode1);
                        dueDate.AddDays(1);
                        now = dueDate;
                    }
                    else // not checking out book already cked out by other patron
                    {
                        // otherwise ignore, already checked out by this patron
                    }
                }
            }
            else
            {
                if (currentPatron != NoPatron) // check in book
                {
                    holdingService.CheckOut(cts, currentPatron, CheckoutPolicies.BookCheckoutPolicy);
                    holdingsService.Update(holdingService);
                }
                else
                    throw new CheckoutException();
            }
        }

        private void CheckInAndFindPatron(decimal fineAmount, int branchId, HoldingService holdingService)
        {
            var patron = patronsService.FindById(holdingService.HeldByPatronId);
            patron.Fine(fineAmount);
            patronsService.Update(patron);
            holdingService.CheckIn(now, branchId);
            holdingService.Update(holdingService);
        }


        public void CompleteCheckout()
        {
            currentPatron = NoPatron;
        }
    }
}