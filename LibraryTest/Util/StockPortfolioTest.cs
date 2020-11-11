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
            StockPortfolio.PurchaseStock();

            Assert.False(StockPortfolio.IsEmpty());
        }

        [Fact]
        public void ReturnZeroSymbolCountWhenNothingIsPurchased()
        {
            var StockPortfolio = new StockPortfolio();

            Assert.Equal(0,StockPortfolio.Count());
        }

    }
}
