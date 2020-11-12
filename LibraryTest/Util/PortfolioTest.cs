using Xunit;
using Assert = Xunit.Assert;
using LibraryNet2020.Util;
using NuGet.Frameworks;
using System;

namespace LibraryCoreTests.Util
{
    public class PortfolioTests
    {
        Portfolio portfolio = new Portfolio();

        [Fact]
        public void PortfolioIsEmpty()
        {
            var isEmpty = portfolio.IsEmpty;

            Assert.True(isEmpty);
        }

        [Fact]
        public void PortfolioNotEmptyAfterPurchase()
        {
            portfolio.Purchase("BAYER");

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
            portfolio.Purchase("BAYER");
            Assert.Equal(1, portfolio.SymbolCount);
        }

        [Fact]
        public void HasSymbolsAfterPurchase()
        {
            portfolio.Purchase("BAYER");
            portfolio.Purchase("APPL");

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
            portfolio.Purchase("GOOGLE");

            Assert.Equal(1, portfolio.GetSharesOfSymbol("GOOGLE"));
            Assert.Equal(0, portfolio.GetSharesOfSymbol("APPL"));
        }

        [Fact]
        public void PurchaseMultipleSharesShouldIncreaseTotalShareCount()
        {
            portfolio.Purchase("BAYER", 1);
            portfolio.Purchase("BAYER", 100);
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
                portfolio.Purchase("BAYER");
                portfolio.Sell("BAYER");
                portfolio.Sell("BAYER");
            });
        }
    }
}

/*
class Portfolio
    Is it empty?
    How Many unique symbols?
    Given a symbol and # of shares, make a purchase
    How many shares for a given symbol?
    Given a symbol and a # of shares, sell the shares
    Throw an exception when selling too many shares


PortfolioIsEmpty
    return true;

PortfolioNotEmptyAfterPurchase
    void Purchase(string symbol, int shares) {}
    return false;
*/

