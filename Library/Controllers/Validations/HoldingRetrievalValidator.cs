using LibraryNet2020.Models;
using static LibraryNet2020.Controllers.Validations.Constants;

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
            Data[HoldingKey] = new HoldingsService(context).FindByBarcode(Barcode);
        }

        public override bool IsValid => Data[HoldingKey] != null;
        public override string ErrorMessage => $"Holding with barcode {Barcode} not found";
    }
}
