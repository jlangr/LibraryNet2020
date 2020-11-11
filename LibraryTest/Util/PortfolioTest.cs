using LibraryNet2020.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LibraryTest.Util
{
    public class PortfolioTest
    {
        private Portfolio portfolio;
        public PortfolioTest()
        {
            portfolio = new Portfolio();
        }

        [Fact]
        public void IsEmptyWhenNothingPurchased()
        {
            Assert.True(portfolio.IsEmpty);
        }

        [Fact]
        public void IsNoLongerEmptyAfterPurchase()
        {
            portfolio.Purchase();

            Assert.False(portfolio.IsEmpty);
        }

        [Fact]
        public void SymbolCountIsZeroWhenNothingPurchased()
        {
            Assert.Equal(0, portfolio.Count);
        }
    }
}
