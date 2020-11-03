using System.Collections.Generic;
using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers.Validations
{
    public abstract class Validator
    {
        protected readonly LibraryContext context;
        public virtual Dictionary<string,object> Data { get; set; } = new Dictionary<string, object>();

        protected Validator()
        {
        }

        protected Validator(LibraryContext context)
        {
            this.context = context;
        }

        public abstract void Validate();
        public abstract bool IsValid { get; }
        public abstract string ErrorMessage { get; }
        public virtual string InvertMessage { get; }

        public virtual void MergePreviousValidationData(Dictionary<string, object> data)
        {
            if (data == null)
                return;
            foreach (var (key, value) in data)
                Data[key] = value;
        }
    }
}