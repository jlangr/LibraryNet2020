using System;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using LibraryNet2020.Scanner;
using LibraryNet2020.Services;
using Moq;
using Xunit;
using static LibraryTest.Services.ServiceHelpers;

namespace LibraryTest.Scanner
{
    [Collection("SharedLibraryContext")]
    public class ScanStation_WhenNewMaterialCheckedOutTest
    {
        readonly DateTime now = DateTime.Now;
        
        ScanStation scanner;
        private LibraryContext context;
        private HoldingsService holdingsService;
        private PatronsService patronsService;
        private Mock<IClassificationService> classificationServiceMock;
        private IClassificationService classificationService;
        private int savedPatronId = 1;
        const string SomeBarcode = "QA123:1";

        public ScanStation_WhenNewMaterialCheckedOutTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            holdingsService = new HoldingsService(context);
            patronsService = new PatronsService(context);
            
            classificationServiceMock = new Mock<IClassificationService>();
            classificationService = classificationServiceMock.Object;
            
            scanner = new ScanStation(context, 1, classificationService, holdingsService, patronsService);
            
            savedPatronId = patronsService.Create(new Patron { Name = ""});
            
            CheckOutNewMaterial();
        }

        private void CheckOutNewMaterial()
        {
            scanner.ScanNewMaterial(SomeBarcode, classificationServiceMock);
            scanner.CheckOut(SomeBarcode, savedPatronId, now);
        }

        [Fact]
        public void HeldByPatronIdUpdated()
        {
            Assert.Equal(savedPatronId, holdingsService.FindByBarcode(SomeBarcode).HeldByPatronId);
        }
        
        [Fact]
        public void CheckOutTimestampUpdated()
        {
            Assert.Equal(now, holdingsService.FindByBarcode(SomeBarcode).CheckOutTimestamp);
        }
        
        [Fact]
        public void IsCheckedOutMarkedTrue()
        {
            Assert.True(holdingsService.FindByBarcode(SomeBarcode).IsCheckedOut);
        }
        
        [Fact]
        public void RescanBySamePatronIsIgnored()
        {
            scanner.AcceptBarcode(SomeBarcode);
        
            Assert.Equal(savedPatronId, holdingsService.FindByBarcode(SomeBarcode).HeldByPatronId);
        }
        
        [Fact]
        public void SecondMaterialCheckedOutAddedToPatron()
        {
            scanner.ScanNewMaterial("XX123:1", classificationServiceMock);
        
            scanner.CheckOut("XX123:1", savedPatronId, now);
        
            Assert.Equal(savedPatronId, holdingsService.FindByBarcode(SomeBarcode).HeldByPatronId);
            Assert.Equal(savedPatronId, holdingsService.FindByBarcode("XX123:1").HeldByPatronId);
        }
        
        [Fact]
        public void SecondPatronCanCheckOutSecondCopyOfSameClassification()
        {
            var barcode1Copy2 = Holding.GenerateBarcode(Holding.ClassificationFromBarcode(SomeBarcode), 2);
            scanner.ScanNewMaterial(barcode1Copy2, classificationServiceMock);
        
            var patronId2 = patronsService.Create(new Patron { Name = "" });
            scanner.AcceptLibraryCard(patronId2);
            scanner.AcceptBarcode(barcode1Copy2);
        
            Assert.Equal(patronId2, holdingsService.FindByBarcode(barcode1Copy2).HeldByPatronId);
        }
        
        [Fact]
        public void CheckInAtSecondBranchResultsInTransfer()
        {
            var newBranchId = scanner.BranchId + 1;
            var scannerBranch2 = new ScanStation(context, newBranchId, classificationService, holdingsService, patronsService);
        
            scannerBranch2.AcceptBarcode(SomeBarcode);
        
            Assert.Equal(newBranchId, holdingsService.FindByBarcode(SomeBarcode).BranchId);
        }
        
        [Fact]
        public void LateCheckInResultsInFine()
        {
            scanner.CompleteCheckout();
            const int daysLate = 2;
        
            scanner.CheckIn(SomeBarcode, DaysPastDueDate(SomeBarcode, now, daysLate, classificationService));
        
            Assert.Equal(RetrievePolicy(SomeBarcode, classificationService).FineAmount(daysLate), patronsService.FindById(savedPatronId).Balance);
        }

        [Fact]
        public void CheckoutByOtherPatronSucceeds()
        {
            scanner.CompleteCheckout();
            var anotherPatronId = patronsService.Create(new Patron {Name = ""});
            scanner.AcceptLibraryCard(anotherPatronId);
        
            scanner.CheckOut(SomeBarcode, anotherPatronId, now);
        
            Assert.Equal(anotherPatronId, holdingsService.FindByBarcode(SomeBarcode).HeldByPatronId);
        }
        
        [Fact]
        public void CheckoutByOtherPatronAssessesAnyFineOnFirst()
        {
            scanner.CompleteCheckout();
            var anotherPatronId = patronsService.Create(new Patron { Name = "" });
        
            const int daysLate = 2;
            scanner.CheckOut(SomeBarcode, anotherPatronId, DaysPastDueDate(SomeBarcode, now, daysLate, classificationService));
        
            Assert.Equal(RetrievePolicy(SomeBarcode, classificationService).FineAmount(daysLate),
                patronsService.FindById(savedPatronId).Balance);
        }
    }
}