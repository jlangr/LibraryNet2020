using System.Collections.Generic;

namespace LibraryNet2020.Controllers.Validations
{
    public class NotValidator : Validator
    {
        private readonly Validator validator;

        public NotValidator(Validator validator)
        {
            this.validator = validator;
        }

        public override void Validate()
        {
            validator.Validate();
        }

        public override bool IsValid => !validator.IsValid;
        public override string ErrorMessage => validator.InvertMessage;
        public override void MergePreviousValidationData(Dictionary<string, object> data)
        {
            validator.MergePreviousValidationData(data);
            Data = validator.Data;
        }
    }
}