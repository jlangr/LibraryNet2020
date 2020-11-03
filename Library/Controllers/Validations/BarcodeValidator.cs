using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers.Validations
{
    public class BarcodeValidator : Validator
    {
        private string Barcode { get; }
        private bool isBarcodeValid;

        public BarcodeValidator(LibraryContext context, string barcode)
            : base(context) =>
            Barcode = barcode;

        public override bool IsValid => isBarcodeValid;

        public override void Validate()
        {
            isBarcodeValid = Holding.IsBarcodeValid(Barcode);
        }

        public override string ErrorMessage => $"Invalid holding barcode format: {Barcode}";
    }
}