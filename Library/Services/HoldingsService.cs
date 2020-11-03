using System;
using System.Linq;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using LibraryNet2020.Util;

namespace LibraryNet2020.Controllers
{
    public class HoldingsService
    {
        private readonly LibraryContext context;
        public const string ErrorMessageDuplicateBarcode = "Duplicate classification / copy number combination.";

        public HoldingsService() {} // needed for Moq

        public HoldingsService(LibraryContext context)
        {
            this.context = context;
        }

        public Holding Add(Holding holding)
        {
            if (holding.CopyNumber == 0)
                holding.CopyNumber = new HoldingsService(context).NextAvailableCopyNumber(holding.Classification);
            else
                ThrowOnDuplicateBarcode(holding);

            var savedHolding = context.Add(holding).Entity;
            context.SaveChanges();
            return savedHolding;
        }

        private void ThrowOnDuplicateBarcode(Holding holding)
        {
            if (new HoldingsService(context).FindByBarcode(holding.Barcode) != null)
                throw new InvalidOperationException(ErrorMessageDuplicateBarcode);
        }

        public Holding FindByClassificationAndCopy(string classification, int copyNumber)
        {
            return context.Holdings
                .FirstOrDefault(h => h.Classification == classification && h.CopyNumber == copyNumber);
        }

        public Holding FindByBarcode(string barcode)
        {
            var (classification, copy) = Holding.BarcodeParts(barcode);
            return FindByClassificationAndCopy(classification, copy);
        }

        public int NextAvailableCopyNumber(string classification)
        {
            return context.Holdings.Count(h => h.Classification == classification) + 1;
        }

        public virtual void CheckOut(Holding holding, int patronId)
        {
            // TODO determine policy material, which in turn comes from from Isbn lookup on creation 
            // Currently Holding creation in controller does not accept ISBN
            holding.CheckOut(TimeService.Now, patronId, new BookCheckoutPolicy());
            context.SaveChanges();
        }

        public virtual void CheckIn(Holding holding, int branchId)
        {
            holding.CheckIn(TimeService.Now, branchId);
            context.SaveChanges();
        }
    }
}