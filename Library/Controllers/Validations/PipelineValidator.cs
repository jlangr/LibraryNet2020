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
            Data = null;
            foreach (var validator in validators)
            {
                Merge(validator.Data, Data);
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

        private void Merge(Dictionary<string, object> d1, Dictionary<string, object> d2)
        {
            if (d2 == null || d1 == null)
                return;
            foreach (var entry in d2)
                d1[entry.Key] = entry.Value;
        }

        public bool IsValid()
        {
            return ErrorMessages.Count == 0;
        }
    }
}