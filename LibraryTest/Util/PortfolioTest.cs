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
        private const decimal CurrentSharePrice = 12;
        private const string BayerSymbol = "BAYN";

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

        [Fact]
        public void SymbolCountIsNotZeroAfterPurchase()
        {
            portfolio.Purchase();

            Assert.NotEqual(0, portfolio.Count);
        }

        [Fact]
        public void SymbolCountIncrementsWithPurchaseOfUniqueSymbol()
        {
            portfolio.Purchase("yolo");
            var countBeforeUniquePurchase = portfolio.Count;

            portfolio.Purchase("notyolo");

            Assert.Equal(countBeforeUniquePurchase + 1, portfolio.Count);
        }

        [Fact]
        public void SymbolCountDoesNotIncrementWithPurchaseOfSameSymbol()
        {
            portfolio.Purchase("yolo");
            var countBeforeUniquePurchase = portfolio.Count;

            portfolio.Purchase("yolo");

            Assert.Equal(countBeforeUniquePurchase, portfolio.Count);
        }

        [Fact]
        public void ValueIsZeroWhenPortfolioIsEmpty()
        {
            Assert.Equal(0, portfolio.Value);
        }

        public class MockStockPriceService : IStockPriceService
        {
            public decimal GetPrice(string symbol)
            {
                return CurrentSharePrice;
            }
        }

        [Fact]
        public void ValueIsEqualToSharePriceAfterSingleSharePurchase()
        {
            portfolio.StockPriceService = new MockStockPriceService();

            portfolio.Purchase(BayerSymbol);

            Assert.Equal(CurrentSharePrice, portfolio.Value);
        }

        [Fact]
        public void ValueContainsTotalWhenMultipleSharesPurchased()
        {
            portfolio.StockPriceService = new MockStockPriceService();

            portfolio.Purchase(BayerSymbol);
            portfolio.Purchase(BayerSymbol);

            Assert.Equal(CurrentSharePrice * 2, portfolio.Value);
        }
    }    
}
