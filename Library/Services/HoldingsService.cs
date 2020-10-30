using System;
using System.Linq;
using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers
{
    public class HoldingsService
    {
        private readonly LibraryContext context;
        public const string ErrorMessageDuplicateBarcode = "Duplicate classification / copy number combination.";

        public HoldingsService(LibraryContext context)
        {
            this.context = context;
        }

        public Holding Add(Holding holding)
        {
            if (holding.CopyNumber == 0)
                holding.CopyNumber = HoldingsControllerUtil.NextAvailableCopyNumber(context, holding.Classification);
            else
                ThrowOnDuplicateBarcode(holding);

            var savedHolding = context.Add(holding).Entity;
            context.SaveChanges();
            return savedHolding;
        }

        private void ThrowOnDuplicateBarcode(Holding holding)
        {
            if (HoldingsControllerUtil.FindByBarcode(context, holding.Barcode) != null)
                throw new InvalidOperationException(ErrorMessageDuplicateBarcode);
        }

        // TODO remove HoldingsControllerUtil
        // TODO null test
        public Holding FindByClassificationAndCopy(string classification, int copyNumber)
        {
            return context.Holdings
                .FirstOrDefault(h => h.Classification == classification && h.CopyNumber == copyNumber);
        }

        // TODO null test
        public Holding FindByBarcode(string barcode)
        {
            return FindByClassificationAndCopy(Holding.ClassificationFromBarcode(barcode), Holding.CopyNumberFromBarcode(barcode));
        }

        // TODO null test
        public int NextAvailableCopyNumber(string classification)
        {
            return context.Holdings.Count(h => h.Classification == classification) + 1;
        }
    }
}