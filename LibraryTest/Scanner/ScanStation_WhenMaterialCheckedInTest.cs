using System;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using LibraryNet2020.Scanner;
using LibraryNet2020.Services;
using LibraryNet2020.Util;
using Moq;
using Xunit;

namespace LibraryTest.Scanner
{
    [Collection("SharedLibraryContext")]
    public class ScanStation_WhenMaterialCheckedInTest
    {
        private const string SomeBarcode = "QA123:1";
        private readonly int somePatronId;
        
        private readonly DateTime now = DateTime.Now;
        
        private LibraryContext context;
        
        private IClassificationService classificationService;
        Mock<IClassificationService> classificationServiceMock;
        private PatronsService patronsService;
        private HoldingsService holdingsService;
        ScanStation scanner;
        
        public ScanStation_WhenMaterialCheckedInTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            patronsService = new PatronsService(context);
            holdingsService = new HoldingsService(context);
            
            classificationServiceMock = new Mock<IClassificationService>();
            classificationService = classificationServiceMock.Object;
            
            somePatronId = patronsService.Create(new Patron {Name = "x"});
            
            scanner = new ScanStation(context, 1, classificationService, holdingsService, patronsService);
            
            scanner.ScanNewMaterial(SomeBarcode, classificationServiceMock);
            scanner.CheckOut(SomeBarcode, somePatronId, now);
            scanner.CompleteCheckout();
            scanner.CheckIn(SomeBarcode, now);
        }

        [Fact]
        public void PatronCleared()
        {
            Assert.Equal(Holding.NoPatron, holdingsService.FindByBarcode(SomeBarcode).HeldByPatronId);
        }

        [Fact]
        public void HoldingMarkedAsNotCheckedOut()
        {
            Assert.False(holdingsService.FindByBarcode(SomeBarcode).IsCheckedOut);
        }

        [Fact]
        public void CheckOutTimestampCleared()
        {
            Assert.Null(holdingsService.FindByBarcode(SomeBarcode).CheckOutTimestamp);
        }

        [Fact]
        public void LastCheckedInTimestampUpdated()
        {
            Assert.Equal(now, holdingsService.FindByBarcode(SomeBarcode).LastCheckedIn);
        }
    }
}