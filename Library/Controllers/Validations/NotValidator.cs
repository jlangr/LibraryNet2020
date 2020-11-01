namespace LibraryNet2020.Controllers.Validations
{
    public class NotValidator : Validator
    {
        public override void Validate()
        {
        }

        public override bool IsValid { get; }
        public override string ErrorMessage { get; }
    }
}