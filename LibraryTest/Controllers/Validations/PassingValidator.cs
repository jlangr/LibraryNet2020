using System.Collections.Generic;
using LibraryNet2020.Controllers.Validations;

namespace LibraryTest.Controllers.Validations
{
    public sealed class PassingValidator : Validator
    {
        public PassingValidator(Dictionary<string, object> data = null)
        {
            Data = data;
        }

        public override void Validate()
        {
        }

        public override bool IsValid => true;
        public override string ErrorMessage => "";
    }
}