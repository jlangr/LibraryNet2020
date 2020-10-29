using System;
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

        // TODO remote HoldingsControllerUtil
        public static Holding FindByClassificationAndCopy(LibraryContext context, string classification, int copyNumber)
        {
            return HoldingsControllerUtil.FindByClassificationAndCopy(context, classification, copyNumber);
        }

        public static Holding FindByBarcode(LibraryContext context, string barcode)
        {
            return HoldingsControllerUtil.FindByBarcode(context, barcode);
        }

        public static int NextAvailableCopyNumber(LibraryContext context, string classification)
        {
            return HoldingsControllerUtil.NextAvailableCopyNumber(context, classification);
        }
    }
}