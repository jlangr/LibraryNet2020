using System;

namespace LibraryNet2020.NonPersistentModels
{
    public class LibraryException: ApplicationException
    {
        public LibraryException(string message) : base(message)
        {
        }
    }
}
