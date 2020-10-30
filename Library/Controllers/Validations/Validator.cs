using System.Collections.Generic;
using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers.Validations
{
    public abstract class Validator
    {
        protected readonly LibraryContext context;
        public Dictionary<string,object> Data { get; set; }

        public Validator(LibraryContext context)
        {
            this.context = context;
            Data = new Dictionary<string, object>();
        }

        public abstract void Validate();
        public abstract bool IsValid { get; }
        public abstract string ErrorMessage { get; }
    }
}