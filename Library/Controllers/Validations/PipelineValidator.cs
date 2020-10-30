using System.Collections.Generic;

namespace LibraryNet2020.Controllers.Validations
{
    public class PipelineValidator
    {
        public IList<string> ErrorMessages { get; set; }
        public Dictionary<string,object> Data { get; set; }

        public PipelineValidator()
        {
            ErrorMessages = new List<string>();
        }
        
        public void Validate(List<Validator> validators)
        {
            
            Validator failingValidation = null;
            Data = new Dictionary<string, object>();
            // TODO reduce
            foreach (var validator in validators)
            {
                validator.Data = Data;
                validator.Validate();
                if (!validator.IsValid)
                {
                    failingValidation = validator;
                    break;
                }

                Data = validator.Data;
            }

            if (failingValidation != null) ErrorMessages.Add(failingValidation.ErrorMessage);
        }

        public bool IsValid()
        {
            return ErrorMessages.Count == 0;
        }
    }
}