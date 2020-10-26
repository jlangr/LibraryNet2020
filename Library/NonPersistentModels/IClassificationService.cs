using LibraryNet2020.Models;

namespace LibraryNet2020.NonPersistentModels
{
    public interface IClassificationService
    {
        string Classification(string isbn);

        void AddBook(string classification, string title, string author, string year);

        Material Retrieve(string classification);

        void DeleteAllBooks();

        void AddMovie(string classification, string title, string director, string year);
    }
}