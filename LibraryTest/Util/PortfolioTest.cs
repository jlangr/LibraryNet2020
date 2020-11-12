using Xunit;
using Assert = Xunit.Assert;
using LibraryNet2020.Util;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using Moq;

namespace LibraryCoreTests.Util
{
    public class PortfolioTests
    {
        Portfolio portfolio = new Portfolio();
        public const decimal AppleStockValue = 100.0m;
        public const decimal GoogleStockValue = 200.0m;
        public const string AppleSymbol = "APPL";
        public const string GoogleSymbol = "GOOG";
        public const string BayerSymbol = "BAYER";

        [Fact]
        public void PortfolioIsEmpty()
        {
            var isEmpty = portfolio.IsEmpty;

            Assert.True(isEmpty);
        }

        [Fact]
        public void PortfolioNotEmptyAfterPurchase()
        {
            portfolio.Trade(BayerSymbol);

            Assert.False(portfolio.IsEmpty);
        }

        [Fact]
        public void HasNoSymbolsBeforePurchase()
        {

            Assert.Equal(0, portfolio.SymbolCount);
        }

        [Fact]
        public void HasSymbolAfterPurchase()
        {
            portfolio.Trade(BayerSymbol);
            Assert.Equal(1, portfolio.SymbolCount);
        }

        [Fact]
        public void HasSymbolsAfterPurchase()
        {
            portfolio.Trade(BayerSymbol);
            portfolio.Trade(AppleSymbol);

            Assert.Equal(2, portfolio.SymbolCount);
        }

        [Fact]
        public void ReturnZeroIfUnpurchasedShares()
        {
            Assert.Equal(0, portfolio.GetSharesOfSymbol(AppleSymbol));
        }

        [Fact]
        public void SharesTrackedIndependently()
        {
            portfolio.Trade(GoogleSymbol);

            Assert.Equal(1, portfolio.GetSharesOfSymbol(GoogleSymbol));
            Assert.Equal(0, portfolio.GetSharesOfSymbol(AppleSymbol));
        }

        [Fact]
        public void PurchaseMultipleSharesShouldIncreaseTotalShareCount()
        {
            portfolio.Trade(BayerSymbol, 1);
            portfolio.Trade(BayerSymbol, 100);
            Assert.Equal(101, portfolio.GetSharesOfSymbol(BayerSymbol));
        }

        [Fact]
        public void ThrowExceptionWhenSellingSharesThatIsNotPurchased()
        {
            Assert.Throws<Exception>(() =>
            {
                portfolio.Sell(BayerSymbol);
            });
        }

        [Fact]
        public void ThrowExceptionWhenSellingSharesGreaterThanAmmountPurchased()
        {
            Assert.Throws<Exception>(() =>
            {
                portfolio.Trade(BayerSymbol);
                portfolio.Sell(BayerSymbol);
                portfolio.Sell(BayerSymbol);
            });
        }

        [Fact]
        public void ValueIsZeroWhenPortfolioIsEmpty()
        {
            Assert.Equal(0, portfolio.Value);
        }

        [Fact]
        public void ValueReturnsStockValueWhenOneShareOwned()
        {
            var mockStockService = new Mock<StockService>();
            mockStockService.Setup(mock => mock.GetStockValue(AppleSymbol))
                .Returns(AppleStockValue);
            portfolio.Trade(AppleSymbol, 1);
            portfolio.MyStockService = mockStockService.Object;

            Assert.Equal(AppleStockValue, portfolio.Value);
        }

        [Fact]
        public void ValueReturnsStockValueWhenMultipleSharesOwner()
        {
            var mockStockService = new Mock<StockService>();
            mockStockService.Setup(mock => mock.GetStockValue(AppleSymbol))
                .Returns(AppleStockValue);
            var stockQuantity = 50;
            portfolio.Trade(AppleSymbol, stockQuantity);
            portfolio.MyStockService = mockStockService.Object;

            Assert.Equal(stockQuantity * AppleStockValue, portfolio.Value);
        }

        [Fact]
        public void ValueReturnsCompletePortfolioValue()
        {
            var appleStockQuantity = 25;
            var googleStockQuantity = 50;
            var mockStockService = new Mock<StockService>();
            mockStockService.Setup(mock => mock.GetStockValue(AppleSymbol))
                .Returns(AppleStockValue);
            mockStockService.Setup(mock => mock.GetStockValue(GoogleSymbol))
                .Returns(GoogleStockValue);
            var expectedValue = appleStockQuantity * AppleStockValue + googleStockQuantity * GoogleStockValue;
            portfolio.MyStockService = mockStockService.Object;

            portfolio.Trade(AppleSymbol, appleStockQuantity);
            portfolio.Trade(GoogleSymbol, googleStockQuantity);

            Assert.Equal(expectedValue, portfolio.Value);
        }

        [Fact]
        public void ShareCountDecreasesForPartialSale()
        {
            portfolio.Trade(GoogleSymbol, 2);

            portfolio.Trade(GoogleSymbol, -1);

            Assert.Equal(1, portfolio.GetSharesOfSymbol(GoogleSymbol));

        }

        [Fact]
        public void TradeIsDeniedForInsufficientShares()
        {
            Assert.Throws<Exception>(() => portfolio.Trade(GoogleSymbol, -1));
        }
    }
}

/*
Is it empty?
    Portfolio is empty
    Portfolio not empty after purchase

How many unique symbols?
    Has 0 symbols before purchase
    Has 1 symbol after purchase
    Has n symbols after n purchases for a given symbol
    Count updates after purchase for a given symbol

Given a symbol and # of shares, make a purchase
    Making purchase increases symbol count
    Making purchase increases share count

How many shares of a given symbol?
    Has 0 shares if not purchased
    Has 1 share after purchase of a given symbol
    Count unchanged after purchase of different symbol

Given a symbol and a # of shares, sell the shares
    Throws exception if selling share that has not been purchased
    Throws exception if selling greater than amount purchased
    Share count decreases after partial sale
    Share removed from collection after complete sale
*/
