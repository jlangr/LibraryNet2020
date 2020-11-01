using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryCoreTests.Models
{
    public class MasterClassificationServiceTest
    {
        private IClassificationService service;

        public MasterClassificationServiceTest()
        {
            service = new MasterClassificationService();
            service.DeleteAllBooks();
        }

        [Fact]
        public void AddBook()
        {
            service.AddBook("QA123", "The Trial", "Kafka, Franz", "1927");
            AssertBook(service.Retrieve("QA123"), "QA123", "The Trial", "Kafka, Franz", "1927");
        }

        [Fact]
        public void ReturnsNullWhenBookNotFound()
        {
            Assert.Null(service.Retrieve("QA123"));
        }

        [Fact]
        public void Persists()
        {
            service.AddBook("QA123", "The Trial", "Kafka, Franz", "1927");

            service = new MasterClassificationService();
            AssertBook(service.Retrieve("QA123"), "QA123", "The Trial", "Kafka, Franz", "1927");
        }

        [Fact]
        public void MultipleBooks()
        {
            service.AddBook("QA123", "The Trial", "Kafka, Franz", "1927");
            service.AddBook("PS334", "Agile Java", "Langr, Jeff", "2005");
            AssertBook(service.Retrieve("QA123"), "QA123", "The Trial", "Kafka, Franz", "1927");
            AssertBook(service.Retrieve("PS334"), "PS334", "Agile Java", "Langr, Jeff", "2005");
        }

        [Fact]
        public void AddMovie()
        {
            service.AddMovie("FF223", "Fight Club", "Fincher, David", "1999");
            var material = service.Retrieve("FF223");
            Assert.Equal("FF223", material.Classification);
            Assert.Equal("Fight Club", material.Title);
            Assert.Equal("Fincher, David", material.Director);
            Assert.Equal("1999", material.Year);
        }

        private static void AssertBook(Material material, string classification, string title, string author, string year)
        {
            Assert.Equal(title, material.Title);
            Assert.Equal(author, material.Author);
            Assert.Equal(year, material.Year);
            Assert.Equal(classification, material.Classification);

            Assert.Equal(typeof(BookCheckoutPolicy), material.CheckoutPolicy.GetType());
        }
    }
}
