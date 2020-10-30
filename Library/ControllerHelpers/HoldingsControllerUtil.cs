using System.Linq;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;

namespace LibraryNet2020.ControllerHelpers
{
    public class HoldingsControllerUtil
    {
        // TODO inline all
        public static Holding FindByClassificationAndCopy(LibraryContext context, string classification, int copyNumber)
        {
            return new HoldingsService(context).FindByClassificationAndCopy(classification, copyNumber);
        }

        public static Holding FindByBarcode(LibraryContext context, string barcode)
        {
            return new HoldingsService(context).FindByBarcode(barcode);
        }

        public static int NextAvailableCopyNumber(LibraryContext context, string classification)
        {
            return new HoldingsService(context).NextAvailableCopyNumber(classification);
        }
    }
}