using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.ControllerHelpers
{
    // TODO merge into HoldingsService
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
            context.Holdings.Add(new Holding("AB123:1"));
            context.Holdings.Add(new Holding("AB123:2"));
            context.Holdings.Add(new Holding("XX123:1"));
            context.SaveChanges();

            var copyNumber = HoldingsControllerUtil.NextAvailableCopyNumber(context, "AB123");

            Assert.Equal(3, copyNumber);
        }

            int idForAB123_2;
            int idForXX123_1;

            private void AddThreeHoldings()
            {
                context.Holdings.Add(new Holding("AB123:1"));
                var e1 = context.Holdings.Add(new Holding("AB123:2")).Entity;
                var e2 = context.Holdings.Add(new Holding("XX123:1")).Entity;
                context.SaveChanges();
                idForAB123_2 = e1.Id;
                idForXX123_1 = e2.Id;
            }

            [Fact]
            public void ByBarcodeReturnsMatchingHolding()
            {
                AddThreeHoldings();
                Assert.Equal(idForAB123_2, HoldingsControllerUtil.FindByBarcode(context, "AB123:2").Id);
            }
            
            /*

            [Fact]
            public void ByClassificationAndCopyReturnsMatchingHolding()
            {
                Assert.Equal(idForXX123_1,
                    HoldingsControllerUtil.FindByClassificationAndCopy(context, "XX123", 1).Id);
            }
        */
    }
}