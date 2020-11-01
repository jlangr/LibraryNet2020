using LibraryNet2020.Controllers.Validations;
using LibraryNet2020.Models;
using static LibraryNet2020.Controllers.Validations.Constants;

namespace LibraryNet2020.Services
{
    public class HoldingAvailableValidator : Validator
    {
        private Holding Holding { get; set; }

        public HoldingAvailableValidator(LibraryContext context)
            : base(context)
        {
        }

        public override void Validate()
        {
            Holding = Data[HoldingKey] as Holding;
        }

        public override bool IsValid => !Holding.IsCheckedOut;
        public override string ErrorMessage => $"Holding with barcode {Holding.Barcode} is already checked out.";
    }
}