using System.Linq;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.Services;
using LibraryNet2020.ViewModels;
using Moq;
using Xunit;
using static LibraryTest.Services.ServiceHelpers;

namespace LibraryTest.Services
{
    [Collection("SharedLibraryContext")]
    public class CheckOutServiceTest
    {
        private readonly LibraryContext context;
        private Mock<HoldingsService> holdingsServiceMock = new Mock<HoldingsService>();

        public CheckOutServiceTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }

        [Fact]
        public void ChecksOutMaterial()
        {
            var patron = SavePatronWithId(context, 5);
            var holding = SaveCheckedInHoldingWithClassification(context, "QA123");
            var checkOutService = new CheckOutService(context, holdingsServiceMock.Object);
            var checkout = new CheckOutViewModel { Barcode = "QA123:1", PatronId = patron.Id };
            
            var result = checkOutService.Checkout(checkout);
            
            Assert.True(result);
            holdingsServiceMock.Verify(s => s.CheckOut(holding, 5));
        }

        [Fact]
        public void ValidatesPatronId()
        {
            var checkOutService = new CheckOutService(context, holdingsServiceMock.Object);
            var checkout = new CheckOutViewModel { Barcode = "QA123:1", PatronId = 1 };

            Assert.False(checkOutService.Checkout(checkout));
            
            Assert.Equal("Patron with ID 1 not found", checkOutService.ErrorMessages.First());
        }

        [Fact]
        public void ValidatesBarcode()
        {
            SavePatronWithId(context, 5);
            var checkOutService = new CheckOutService(context, holdingsServiceMock.Object);
            var checkout = new CheckOutViewModel { Barcode = "QA1231", PatronId = 5 };

            Assert.False(checkOutService.Checkout(checkout));
            
            Assert.Equal("Invalid holding barcode format: QA1231", checkOutService.ErrorMessages.First());
        }

        [Fact]
        public void ValidatesHoldingRetrieval()
        {
            SavePatronWithId(context, 5);
            var checkOutService = new CheckOutService(context, holdingsServiceMock.Object);
            var checkout = new CheckOutViewModel { Barcode = "QA123:1", PatronId = 5 };

            Assert.False(checkOutService.Checkout(checkout));
            
            Assert.Equal("Holding with barcode QA123:1 not found", 
                checkOutService.ErrorMessages.First());
        }

        [Fact]
        public void ValidatesHoldingAvailability()
        {
            SaveCheckedOutHoldingWithClassification(context, "QA123");
            SavePatronWithId(context, 5);
            var checkOutService = new CheckOutService(context, holdingsServiceMock.Object);
            var checkout = new CheckOutViewModel { Barcode = "QA123:1", PatronId = 5 };

            Assert.False(checkOutService.Checkout(checkout));
            
            Assert.Equal("Holding with barcode QA123:1 is already checked out.", 
                checkOutService.ErrorMessages.First());
        }
    }
}