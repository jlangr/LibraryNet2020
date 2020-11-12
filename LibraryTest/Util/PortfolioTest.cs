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
            portfolio.Purchase("yolo", 1);

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
            portfolio.Purchase("yolo", 1);

            Assert.NotEqual(0, portfolio.Count);
        }

        [Fact]
        public void SymbolCountIncrementsWithPurchaseOfUniqueSymbol()
        {
            portfolio.Purchase("yolo", 1);
            var countBeforeUniquePurchase = portfolio.Count;

            portfolio.Purchase("notyolo", 1);

            Assert.Equal(countBeforeUniquePurchase + 1, portfolio.Count);
        }

        [Fact]
        public void SymbolCountDoesNotIncrementWithPurchaseOfSameSymbol()
        {
            portfolio.Purchase("yolo", 1);
            var countBeforeUniquePurchase = portfolio.Count;

            portfolio.Purchase("yolo", 1);

            Assert.Equal(countBeforeUniquePurchase, portfolio.Count);
        }

        [Fact]
        public void ValueIsZeroWhenPortfolioIsEmpty()
        {
            Assert.Equal(0, portfolio.GetPortfolioValue());
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

            portfolio.Purchase(BayerSymbol, 1);

            Assert.Equal(CurrentSharePrice, portfolio.GetPortfolioValue());
        }

        [Fact]
        public void ValueContainsTotalWhenMultipleSharesPurchased()
        {
            portfolio.StockPriceService = new MockStockPriceService();

            portfolio.Purchase(BayerSymbol, 2);            

            Assert.Equal(CurrentSharePrice * 2, portfolio.GetPortfolioValue());
        }
        [Fact]      
        public void ValueContainsTotalAfterMultiplePurchases()
        {
            portfolio.StockPriceService = new MockStockPriceService();

            portfolio.Purchase(BayerSymbol, 2);
            portfolio.Purchase(BayerSymbol, 2);

            Assert.Equal(CurrentSharePrice * 4, portfolio.GetPortfolioValue());
        }
    }    
}
