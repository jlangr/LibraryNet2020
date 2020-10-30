using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers.Validations
{
    public class HoldingRetrievalValidator: Validator
    {
        private string Barcode { get; }
        
        public HoldingRetrievalValidator(LibraryContext context, string barcode)
            : base(context)
            => Barcode = barcode;

        public override void Validate()
        {
            // TODO constant
            Data["Holding"] = HoldingsControllerUtil.FindByBarcode(context, Barcode);
        }

        public override bool IsValid => Data["Holding"] != null;
        public override string ErrorMessage => $"Holding with barcode {Barcode} not found";
    }
}