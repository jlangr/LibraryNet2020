using Xunit;
using Assert = Xunit.Assert;
using LibraryNet2020.Util;

namespace LibraryCoreTests.Util {
    public class PortfolioTests {
        Portfolio portfolio = new Portfolio();

        [Fact]
        public void PortfolioIsEmpty() {
            var isEmpty = portfolio.IsEmpty;

            Assert.True(isEmpty);
        }

        [Fact]
        public void PortfolioNotEmptyAfterPurchase() {
            portfolio.Purchase();

            Assert.False(portfolio.IsEmpty);
        }

        [Fact]
        public void HasNoSymbolsBeforePurchase() {
            
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

