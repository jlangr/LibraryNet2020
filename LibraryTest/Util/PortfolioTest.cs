using Xunit;
using Assert = Xunit.Assert;
using LibraryNet2020.Util;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;

namespace LibraryCoreTests.Util
{
    public class PortfolioTests
    {
        Portfolio portfolio = new Portfolio();
        public const decimal AppleStockValue = 100.0m;
        public const decimal GoogleStockValue = 200.0m;

        public class StubStockService : StockService
        {
            private Dictionary<string, decimal> StockFixture = new Dictionary<string, decimal> { { "GOOG", GoogleStockValue }, { "APPL", AppleStockValue } };

            public decimal GetStockValue(string symbol)
            {
                return StockFixture.ContainsKey(symbol) ? StockFixture[symbol] : 0;
            }
        }

        [Fact]
        public void PortfolioIsEmpty()
        {
            var isEmpty = portfolio.IsEmpty;

            Assert.True(isEmpty);
        }

        [Fact]
        public void PortfolioNotEmptyAfterPurchase()
        {
            portfolio.Trade("BAYER");

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
            portfolio.Trade("BAYER");
            Assert.Equal(1, portfolio.SymbolCount);
        }

        [Fact]
        public void HasSymbolsAfterPurchase()
        {
            portfolio.Trade("BAYER");
            portfolio.Trade("APPL");

            Assert.Equal(2, portfolio.SymbolCount);
        }

        [Fact]
        public void ReturnZeroIfUnpurchasedShares()
        {
            Assert.Equal(0, portfolio.GetSharesOfSymbol("APPL"));
        }

        [Fact]
        public void SharesTrackedIndependently()
        {
            portfolio.Trade("GOOGLE");

            Assert.Equal(1, portfolio.GetSharesOfSymbol("GOOGLE"));
            Assert.Equal(0, portfolio.GetSharesOfSymbol("APPL"));
        }

        [Fact]
        public void PurchaseMultipleSharesShouldIncreaseTotalShareCount()
        {
            portfolio.Trade("BAYER", 1);
            portfolio.Trade("BAYER", 100);
            Assert.Equal(101, portfolio.GetSharesOfSymbol("BAYER"));
        }

        [Fact]
        public void ThrowExceptionWhenSellingSharesThatIsNotPurchased()
        {
            Assert.Throws<Exception>(() =>
            {
                portfolio.Sell("BAYER");
            });
        }

        [Fact]
        public void ThrowExceptionWhenSellingSharesGreaterThanAmmountPurchased()
        {
            Assert.Throws<Exception>(() =>
            {
                portfolio.Trade("BAYER");
                portfolio.Sell("BAYER");
                portfolio.Sell("BAYER");
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
            portfolio.Trade("APPL", 1);
            portfolio.MyStockService = new StubStockService();

            Assert.Equal(AppleStockValue, portfolio.Value);
        }

        [Fact]
        public void ValueReturnsStockValueWhenMultipleSharesOwner()
        {
            var stockQuantity = 50;
            portfolio.Trade("APPL", stockQuantity);
            portfolio.MyStockService = new StubStockService();

            Assert.Equal(stockQuantity * AppleStockValue, portfolio.Value);
        }

        [Fact]
        public void ValueReturnsCompletePortfolioValue()
        {
            var appleStockQuantity = 25;
            var googleStockQuantity = 50;
            portfolio.Trade("APPL", appleStockQuantity);
            portfolio.Trade("GOOG", googleStockQuantity);
            portfolio.MyStockService = new StubStockService();
            var expectedValue = appleStockQuantity * AppleStockValue + googleStockQuantity * GoogleStockValue;

            Assert.Equal(expectedValue, portfolio.Value);
        }

        [Fact]
        public void ShareCountDecreasesForPartialSale()
        {            
            portfolio.Trade("GOOG", 2);

            portfolio.Trade("GOOG", -1);

            Assert.Equal(1, portfolio.GetSharesOfSymbol("GOOG"));

        }

        [Fact]
        public void TradeIsDeniedForInsufficientShares()
        {
            Assert.Throws<Exception>(() => portfolio.Trade("GOOG", -1));
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
