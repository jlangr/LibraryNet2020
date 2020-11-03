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
    }
}