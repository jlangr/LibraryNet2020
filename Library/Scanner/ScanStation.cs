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
        public void AcceptBarcode(string barcode)
        {
            var h = holdingsService.FindByBarcode(barcode);

            if (h.IsCheckedOut)
            {
                if (cur == NoPatron)
                {
                    // ci
                    barcode = h.Barcode;
                    var patronId = h.HeldByPatronId;
                    var cis = TimeService.Now;
                    Material m = null;
                    m = classificationService.Retrieve(h.Classification);
                    var fine = m.CheckoutPolicy.FineAmount(h.CheckOutTimestamp.Value, cis);
                    Patron p = patronsService.FindById(patronId);
                    p.Fine(fine);
                    patronsService.Update(p);
                    h.CheckIn(cis, brId);
                    holdingsService.Update(h);
                }
                else
                {
                    if (h.HeldByPatronId != cur) // check out book already cked-out
                    {
                        var bc1 = h.Barcode;
                        var n = TimeService.Now;
                        var t = TimeService.Now.AddDays(21);
                        var f = classificationService.Retrieve(h.Classification).CheckoutPolicy
                            .FineAmount(h.CheckOutTimestamp.Value, n);
                        var patron = patronsService.FindById(h.HeldByPatronId);
                        patron.Fine(f);
                        patronsService.Update(patron);
                        h.CheckIn(n, brId);
                        holdingsService.Update(h);
                        // co
                        h.CheckOut(n, cur, CheckoutPolicies.BookCheckoutPolicy);
                        holdingsService.Update(h);
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
                    h.CheckOut(cts, cur, CheckoutPolicies.BookCheckoutPolicy);
                    holdingsService.Update(h);
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