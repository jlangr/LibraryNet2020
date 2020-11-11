using System;
using LibraryNet2020.Util;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Util
{
    public class StockPortfolioTest
    {
        public StockPortfolioTest()
        {
        }

        [Fact]
        public void ReturnTrueWhenNoStockPurchased()
        {
            var StockPortfolio = new StockPortfolio();

            Assert.True(StockPortfolio.IsEmpty());
        }

        [Fact]
        public void ReturnFalseAfterStockPurchased()
        {
            var StockPortfolio = new StockPortfolio();
            StockPortfolio.PurchaseStock("TST");

            Assert.False(StockPortfolio.IsEmpty());
        }

        [Fact]
        public void ReturnZeroSymbolCountWhenNothingIsPurchased()
        {
            var StockPortfolio = new StockPortfolio();

            Assert.Equal(0,StockPortfolio.Count());
        }

        [Fact]
        public void ReturnOneSymbolCountWhenPurchased()
        {
            var StockPortfolio = new StockPortfolio();
            StockPortfolio.PurchaseStock("TST");

            Assert.Equal(1, StockPortfolio.Count());
        }

        [Fact]
        public void ReturnCountOfUniqueSymbolsPurchased()
        {
            var StockPortfolio = new StockPortfolio();
            StockPortfolio.PurchaseStock("TST-1");
            StockPortfolio.PurchaseStock("TST-2");

            Assert.Equal(2, StockPortfolio.Count());
        }

        //[Fact]
        //public void SymbolCountShouldIncrementForEachUniqueSymbols()
        //{
        //    var StockPortfolio = new StockPortfolio();
        //    StockPortfolio.PurchaseStock("TST-1");
        //    StockPortfolio.PurchaseStock("TST-2");
        //    StockPortfolio.PurchaseStock("TST-2");

        //    Assert.Equal(1, StockPortfolio.Count("TST-1"));
        //    Assert.Equal(2, StockPortfolio.Count("TST-2"));
        //}
    }
}
