using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.ControllerHelpers
{
    // TODO merge into HoldingsServiceTests
    [Collection("SharedLibraryContext")]
    public class HoldingsControllerUtilTest
    {
        LibraryContext context;

        public HoldingsControllerUtilTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }

        [Fact]
        public void NextAvailableCopyNumberIncrementsCopyNumberUsingCount()
        {
            AddNewHolding("AB123:2");
            AddNewHolding("AB123:1");
            AddNewHolding("XX123:1");

            var copyNumber = new HoldingsService(context).NextAvailableCopyNumber("AB123");

            Assert.Equal(3, copyNumber);
        }

        [Fact]
        public void ByBarcodeReturnsMatchingHolding()
        {
            var holding = AddNewHolding("AB123:2");
            AddNewHolding("XX123:1");

            var retrievedHolding = new HoldingsService(context).FindByBarcode("AB123:2");
            
            Assert.Equal(holding.Id, retrievedHolding.Id);
        }

        [Fact]
        public void ByClassificationAndCopyReturnsMatchingHolding()
        {
            AddNewHolding("AB123:2");
            
            var holding = AddNewHolding("XX123:1");
            var retrieved = new HoldingsService(context).FindByClassificationAndCopy("XX123", 1);
            
            Assert.Equal(holding.Id, retrieved.Id);
        }
        
        private Holding AddNewHolding(string classification)
        {
            var entity = context.Holdings.Add(new Holding(classification)).Entity;
            context.SaveChanges();
            return entity;
        }
    }
}