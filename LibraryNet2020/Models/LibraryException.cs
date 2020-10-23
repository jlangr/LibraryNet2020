using System;

namespace LibraryNet2020.Models
{
    public class LibraryException: ApplicationException
    {
        public LibraryException(string message) : base(message)
        {
        }
    }
}
