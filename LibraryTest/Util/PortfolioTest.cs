using System;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Util
{
    public class PortfolioTest
    {
        private const string Bayn = "BAYN";
        private const decimal BaynCurrentPrice = 12.50m;
        private const decimal IbmCurrentPrice = 100.00m;
        private const string Ibm = "IBM";
        private Portfolio portfolio = new Portfolio();
        
        [Fact]
        public void IsEmptyWhenCreated()
        {
            Assert.True(portfolio.IsEmpty);
        }
        
        [Fact]
        public void IsNoLongerEmptyAfterPurchase()
        {
            portfolio.Purchase(Bayn, 10);

            Assert.False(portfolio.IsEmpty);
        }

        [Fact]
        public void SizeIsZeroWhenCreated()
        {
            Assert.Equal(0, portfolio.Size);
        }

        [Fact]
        public void SizeIsNonZeroAfterPurchase()
        {
            portfolio.Purchase(Bayn, 10);
            
            Assert.Equal(1, portfolio.Size);
        }

        [Fact]
        public void SizeIncrementsOnEachPurchaseOfUniqueSymbol()
        {
            portfolio.Purchase(Bayn, 10);
            portfolio.Purchase(Ibm, 10);
            
            Assert.Equal(2, portfolio.Size);
        }

        [Fact]
        public void SizeDoesNotIncrementsOnPurchaseOfSameSymbol()
        {
            portfolio.Purchase(Bayn, 10);
            portfolio.Purchase(Bayn, 10);
            
            Assert.Equal(1, portfolio.Size);
        }

        [Fact]
        public void ReturnsSharesOfPurchasedSymbol()
        {
            portfolio.Purchase(Bayn, 7);
            
            Assert.Equal(7, portfolio.Shares(Bayn));
        }

        [Fact]
        public void ReturnsSharesOnlyOfPurchasedSymbol()
        {
            portfolio.Purchase(Bayn, 7);
            portfolio.Purchase(Ibm, 14);
            
            Assert.Equal(7, portfolio.Shares(Bayn));
        }

        [Fact]
        public void ReturnsZeroForSharesOfUnpurchasedSymbol()
        {
            Assert.Equal(0, portfolio.Shares(Bayn));
        }

        [Fact]
        public void ReturnsShareTotalForMultiplePurchasesOfSymbol()
        {
            portfolio.Purchase(Bayn, 7);
            portfolio.Purchase(Bayn, 14);
            
            Assert.Equal(21, portfolio.Shares(Bayn));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ItThrowsOnNonPositiveShareCount(int sharesToPurchase)
        {
            Assert.Throws<ArgumentException>(() => portfolio.Purchase(Bayn, sharesToPurchase));
        }

        [Fact]
        public void ReducesShareCountOnSell()
        {
            portfolio.Purchase(Bayn, 28);
            portfolio.Sell(Bayn, 21);
            
            Assert.Equal(7, portfolio.Shares(Bayn));
        }

        [Fact]
        public void ThrowsWhenSellingTooManyShares()
        {
            portfolio.Purchase(Bayn, 10);
            Assert.Throws<ArgumentException>(() => portfolio.Sell(Bayn, 11));
        }

        [Fact]
        public void ReducesSizeWhenAllSharesSoldForSymbol()
        {
            portfolio.Purchase(Bayn, 10);
            portfolio.Sell(Bayn, 10);
            
            Assert.Equal(0, portfolio.Size);
        }

        [Fact]
        public void ValueZeroWhenPortfolioIsEmpty()
        {
            Assert.Equal(0, portfolio.Value);
        }

        public class StubStockService : StockService
        {
            public decimal CurrentPrice(string symbol)
            {
                return BaynCurrentPrice;
            }
        }

        [Fact]
        public void ValueEqualsSingleSharePurchased()
        {
            portfolio.StockService = new StubStockService();
            portfolio.Purchase(Bayn, 1);
            Assert.Equal(BaynCurrentPrice, portfolio.Value);
        }

        [Fact]
        public void ValueEqualsMultipleSharesOfSingleSymbol()
        {
            portfolio.StockService = new StubStockService();
            portfolio.Purchase(Bayn, 10);
            Assert.Equal(BaynCurrentPrice*10, portfolio.Value);
        }
        [Fact]
        public void ValueEqualsMultipleSharesOfMultipleSymbol()
        {
            var mock = new Moq.Mock<StockService>();
            mock.Setup(x => x.CurrentPrice(Bayn)).Returns(BaynCurrentPrice);
            mock.Setup(x => x.CurrentPrice(Ibm)).Returns(IbmCurrentPrice);
            portfolio.StockService = mock.Object;

            portfolio.Purchase(Bayn, 10);
            portfolio.Purchase(Ibm, 5);

            Assert.Equal(BaynCurrentPrice * 10 + IbmCurrentPrice *5, portfolio.Value);
        }

    }
}