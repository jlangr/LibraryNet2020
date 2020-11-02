using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;

namespace LibraryTest.Controllers.Validations
{
    public sealed class FailingValidator : Validator
    {
        public FailingValidator(string message = "default message", Dictionary<string, object> data = null)
        {
            ErrorMessage = message;
            Data = data;
        }

        public override void Validate()
        {
        }

        public override bool IsValid => false;
        public override string ErrorMessage { get; }
    }
}