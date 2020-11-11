using System;
using Xunit;

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
    }
}
