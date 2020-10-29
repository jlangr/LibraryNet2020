using System.Linq;
using LibraryNet2020.Models;

namespace LibraryNet2020.ControllerHelpers
{
    public class HoldingsControllerUtil
    {
        // TODO null test
        public static Holding FindByClassificationAndCopy(LibraryContext context, string classification, int copyNumber)
        {
            return context.Holdings
                .FirstOrDefault(h => h.Classification == classification && h.CopyNumber == copyNumber);
        }

        public static Holding FindByBarcode(LibraryContext context, string barcode)
        {
            return FindByClassificationAndCopy(context, Holding.ClassificationFromBarcode(barcode), Holding.CopyNumberFromBarcode(barcode));
        }

        public static int NextAvailableCopyNumber(LibraryContext context, string classification)
        {
            return context.Holdings.Count(h => h.Classification == classification) + 1;
        }
    }
}