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
            Assert.True(StockPortfolio.IsEmpty());
        }

        [Fact]
        public void ReturnFalseAfterStockPurchased()
        {
            Assert.False(StockPortfolio.PurchaseStock());
        }

    }
}
