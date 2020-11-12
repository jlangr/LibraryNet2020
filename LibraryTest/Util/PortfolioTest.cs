using LibraryNet2020.Util;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
namespace LibraryTest.Util
{
    public class PortfolioTest
    {
        private Portfolio portfolio;
        private const decimal CurrentBayerSharePrice = 12;
        private const decimal CurrentOtherSharePrice = 13;
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

        [Fact]
        public void ValueIsEqualToSharePriceAfterSingleSharePurchase()
        {
            var mockService = new Mock<IStockPriceService>();
            mockService.Setup(x => x.GetPrice(It.IsAny<string>())).Returns(CurrentBayerSharePrice);
            portfolio.StockPriceService = mockService.Object;

            portfolio.Purchase(BayerSymbol, 1);

            Assert.Equal(CurrentBayerSharePrice, portfolio.GetPortfolioValue());
        }

        [Fact]
        public void ValueContainsTotalWhenMultipleSharesPurchased()
        {
            var mockService = new Mock<IStockPriceService>();
            mockService.Setup(x => x.GetPrice(It.IsAny<string>())).Returns(CurrentBayerSharePrice);
            portfolio.StockPriceService = mockService.Object;

            portfolio.Purchase(BayerSymbol, 2);

            Assert.Equal(CurrentBayerSharePrice * 2, portfolio.GetPortfolioValue());
        }

        [Fact]
        public void ValueContainsTotalAfterMultiplePurchases()
        {
            var mockService = new Mock<IStockPriceService>();
            mockService.Setup(x => x.GetPrice(It.IsAny<string>())).Returns(CurrentBayerSharePrice);
            portfolio.StockPriceService = mockService.Object;

            portfolio.Purchase(BayerSymbol, 2);
            portfolio.Purchase(BayerSymbol, 2);

            Assert.Equal(CurrentBayerSharePrice * 4, portfolio.GetPortfolioValue());
        }

        [Fact]
        public void ValueContainsTotalAfterDifferentSymbolsPurchased()
        {
            var mockService = new Mock<IStockPriceService>();
            mockService.Setup(x => x.GetPrice(BayerSymbol)).Returns(CurrentBayerSharePrice);
            mockService.Setup(x => x.GetPrice(BayerSymbol + "other")).Returns(CurrentOtherSharePrice);
            portfolio.StockPriceService = mockService.Object;

            portfolio.Purchase(BayerSymbol, 2);
            portfolio.Purchase(BayerSymbol + "other", 2);

            var expectedTotal = CurrentBayerSharePrice * 2 + CurrentOtherSharePrice * 2;
            Assert.Equal(expectedTotal, portfolio.GetPortfolioValue());
        }

        //[Fact]
        //public void ThrowsWhenSellingStockNotOwned()
        //{
        //    portfolio.StockPriceService = new MockStockPriceService();

        //    Assert.Throws<Exception>(() => portfolio.Sell(BayerSymbol, 1));
        //}
    }
}
