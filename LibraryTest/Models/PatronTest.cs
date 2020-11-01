using LibraryNet2020.Models;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Models
{
    public class PatronTest
    {
        const int Id = 101;
        const string Name = "Joe";
        const int HoldingId = 2;
        private Patron patron;

        public PatronTest()
        {
            patron = new Patron(Id, Name);
        }

        [Fact]
        public void BalanceIsZeroOnCreation()
        {
            Assert.Equal(0, patron.Balance);
        }

        [Fact]
        public void FinesIncreaseBalance()
        {
            patron.Fine(0.10m);
            Assert.Equal(0.10m, patron.Balance);
            patron.Fine(0.10m);
            Assert.Equal(0.20m, patron.Balance);
        }

        [Fact]
        public void RemitReducesBalance()
        {
            patron.Fine(1.10m);

            patron.Remit(0.20m);

            Assert.Equal(0.90m, patron.Balance);
        }
    }
}
