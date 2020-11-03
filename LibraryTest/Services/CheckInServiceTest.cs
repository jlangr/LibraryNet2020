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
    public class CheckInServiceTest
    {
        private readonly LibraryContext context;
        private Mock<HoldingsService> holdingsServiceMock = new Mock<HoldingsService>();

        public CheckInServiceTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }
        
        [Fact]
        public void ChecksInBook()
        {
            var holding = SaveCheckedOutHoldingWithClassification(context, "QA123");
            var checkInService = new CheckInService(context, holdingsServiceMock.Object);
            var checkin = new CheckInViewModel { Barcode = "QA123:1", BranchId = 42 };
            
            Assert.True(checkInService.Checkin(checkin));
            
            holdingsServiceMock.Verify(s => s.CheckIn(holding, 42));
        }
        
        [Fact]
        public void ValidatesBarcode()
        {
            var checkInService = new CheckInService(context, holdingsServiceMock.Object);
            var checkin = new CheckInViewModel { Barcode = "QA1231" };

            Assert.False(checkInService.Checkin(checkin));
            
            Assert.Equal("Invalid holding barcode format: QA1231", checkInService.ErrorMessages.First());
        }

        [Fact]
        public void ValidatesHoldingRetrieval()
        {
            SavePatronWithId(context, 5);
            var checkInService = new CheckInService(context, holdingsServiceMock.Object);
            var checkin = new CheckInViewModel { Barcode = "QA123:1" };

            Assert.False(checkInService.Checkin(checkin));
            
            Assert.Equal("Holding with barcode QA123:1 not found", 
                checkInService.ErrorMessages.First());
        }

        [Fact]
        public void ValidatesHoldingAvailability()
        {
            SaveCheckedInHoldingWithClassification(context, "QA123");
            SavePatronWithId(context, 1);
            var checkInService = new CheckInService(context, holdingsServiceMock.Object);
            var checkin = new CheckInViewModel { Barcode = "QA123:1" };

            Assert.False(checkInService.Checkin(checkin));
            
            // TODO Negate!
            Assert.Equal("Holding with barcode QA123:1 is already checked out.", 
                checkInService.ErrorMessages.First());
        }
    }
}