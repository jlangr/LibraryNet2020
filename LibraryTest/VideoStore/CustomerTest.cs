using LibraryCore.VideoStore;
using LibraryNet2020.VideoStore;
using Xunit;
using Assert = Xunit.Assert;
using Env = System.Environment;

namespace LibraryTest.VideoStore
{
    public class CustomerTest
    {
        private readonly Customer customer;
        private const string TAB = "\t";

        public CustomerTest()
        {
            customer = new Customer("Fred");
        }

        [Fact]
        public void SingleNewReleaseStatement()
        {
            customer.Add(new Rental(new Movie("The Cell", Movie.NewRelease), 3));
            Assert.Equal(
                "Rental Record for Fred" + Env.NewLine +
                TAB + "The Cell" + TAB + "9.00" +  Env.NewLine +
                "You owed 9.00" + Env.NewLine + 
                "You earned 2 frequent renter points" + Env.NewLine,
                customer.Statement());
        }

        [Fact]
        public void DualNewReleaseStatement()
        {
            customer.Add(new Rental(new Movie("The Cell", Movie.NewRelease), 3));
            customer.Add(new Rental(new Movie("The Tigger Movie", Movie.NewRelease), 3));
            Assert.Equal(
                "Rental Record for Fred" + Env.NewLine +
                TAB + "The Cell" + TAB + "9.00" + Env.NewLine +
                TAB + "The Tigger Movie" + TAB + "9.00" + Env.NewLine +
                "You owed 18.00" + Env.NewLine +
                "You earned 4 frequent renter points" + Env.NewLine,
                customer.Statement());
        }

        [Fact]
        public void SingleChildrensStatement()
        {
            customer.Add(new Rental(new Movie("The Tigger Movie", Movie.Childrens), 3));
            Assert.Equal(
                "Rental Record for Fred" + Env.NewLine +
                "\tThe Tigger Movie\t1.50" + Env.NewLine +
                "You owed 1.50" + Env.NewLine +
                "You earned 1 frequent renter points" + Env.NewLine,
                customer.Statement());
        }

        [Fact]
        public void MultipleRegularStatement()
        {
            customer.Add(new Rental(new Movie("Plan 9 from Outer Space", Movie.Regular), 1));
            customer.Add(new Rental(new Movie("8 1/2", Movie.Regular), 2));
            customer.Add(new Rental(new Movie("Eraserhead", Movie.Regular), 3));

            Assert.Equal(
                "Rental Record for Fred" + Env.NewLine +
                "\tPlan 9 from Outer Space\t2.00" + Env.NewLine +
                "\t8 1/2\t2.00" + Env.NewLine +
                "\tEraserhead\t3.50" + Env.NewLine +
                "You owed 7.50" + Env.NewLine +
                "You earned 3 frequent renter points" + Env.NewLine,
                customer.Statement());
        }
    }
}
