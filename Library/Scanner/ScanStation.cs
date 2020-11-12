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
        private int cur = NoPatron;
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

        public int CurrentPatronId => cur;

        public void AcceptLibraryCard(int patronId)
        {
            cur = patronId;
            cts = TimeService.Now;
        }

        // 1/19/2017: who wrote this?
        // 
        // FIXME. Fix this mess. We just have to SHIP IT for nwo!!!
        public void AcceptBarcode(string barCode)
        {
            var holding = holdingsService.FindByBarcode(barCode);

            if (holding.IsCheckedOut)
            {
                if (cur == NoPatron)
                {
                    barCode = holding.Barcode;
                    var patronId = holding.HeldByPatronId;
                    var cis = TimeService.Now;
                    Material m = null;
                    m = classificationService.Retrieve(holding.Classification);
                    var fine = m.CheckoutPolicy.FineAmount(holding.CheckOutTimestamp.Value, cis);
                    Patron p = patronsService.FindById(patronId);
                    p.Fine(fine);
                    patronsService.Update(p);
                    holding.CheckIn(cis, brId);
                    holdingsService.Update(holding);
                }
                else
                {
                    if (holding.HeldByPatronId != cur) // check out book already cked-out
                    {
                        var bc1 = holding.Barcode;
                        var n = TimeService.Now;
                        var t = TimeService.Now.AddDays(21);
                        var f = classificationService.Retrieve(holding.Classification).CheckoutPolicy
                            .FineAmount(holding.CheckOutTimestamp.Value, n);
                        var patron = patronsService.FindById(holding.HeldByPatronId);
                        patron.Fine(f);
                        patronsService.Update(patron);
                        holding.CheckIn(n, brId);
                        holdingsService.Update(holding);
                        // co
                        holding.CheckOut(n, cur, CheckoutPolicies.BookCheckoutPolicy);
                        holdingsService.Update(holding);
                        // call check out controller(cur, bc1);
                        t.AddDays(1);
                        n = t;
                    }
                    else // not checking out book already cked out by other patron
                    {
                        // otherwise ignore, already checked out by this patron
                    }
                }
            }
            else
            {
                if (cur != NoPatron) // check in book
                {
                    holding.CheckOut(cts, cur, CheckoutPolicies.BookCheckoutPolicy);
                    holdingsService.Update(holding);
                }
                else
                    throw new CheckoutException();
            }
        }

        public void CompleteCheckout()
        {
            cur = NoPatron;
        }
    }
}