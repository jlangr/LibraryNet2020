using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using Microsoft.IdentityModel.Tokens;

namespace LibraryNet2020.Services
{
    public class BarcodeValidator : Validator
    {
        private string Barcode { get; }
        private readonly LibraryContext context;
        private bool isBarcodeValid;

        public BarcodeValidator(LibraryContext context, string barcode)
            : base(context) =>
            Barcode = barcode;

        public override bool IsValid => isBarcodeValid;

        public override void Validate()
        {
            isBarcodeValid = !Holding.IsBarcodeValid(Barcode);
        }

        public override string ErrorMessage => $"Invalid holding barcode format: {Barcode}";
    }
}