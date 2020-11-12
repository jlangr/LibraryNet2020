using System;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using Xunit;
using Assert = Xunit.Assert;

/*
This test class is a mess. Some of the following opportunities for cleanup might exist:

 - AAA used but no visual separation
 - seeming use of AAA but it's not really
 - duplicate and uninteresting initialization
 - unnecessary code (null checks? try/catch?)
 - things that bury information relevant to the test
 - inconsistent test names
 - test names that don't emphasize behavior
 - comments in tests (are they even true)?
 - multiple behaviors/asserts per test
 - dead code
 - local variables that add no readability or other value
 - variables that don't provide enough relevant info
 */


namespace LibraryTest.Models
{
    public class HoldingTest
    {
        const int PatronId = 101;
        const string ExpectedBarcode = "QA234:3";
        const int SomeBranchId = 1;

        [Fact]
        public void IsCheckedOutWhenBranchIsTheCheckedOutBranch()
        {
            var holding = new Holding { BranchId = Branch.CheckedOutId };
            Assert.True(holding.IsCheckedOut);
            Assert.False(holding.IsAvailable);
        }

        [Fact]
        public void IsAvailableOutWhenBranchIsSet()
        {
            var holding = new Holding { BranchId = 1 };
            Assert.False(holding.IsCheckedOut);
            Assert.True(holding.IsAvailable);
        }

        [Fact]
        public void CreateWithCommonArguments()
        {
            const int branchId = 10;
            var holding = new Holding("QA123", 2, branchId);
            Assert.NotNull(holding);
            Assert.Equal("QA123:2", holding.Barcode);
            Assert.Equal(branchId, holding.BranchId);
        }

        [Fact]
        public void IsValidBarcodeReturnsFalseWhenItHasNoColon()
        {
            Assert.False(Holding.IsBarcodeValid("ABC"));
        }

        [Fact]
        public void IsValidBarcodeReturnsFalseWhenItsCopyNumberNotPositiveInt()
        {
            Assert.False(Holding.IsBarcodeValid("ABC:X"));
            Assert.False(Holding.IsBarcodeValid("ABC:0"));
        }

        [Fact]
        public void IsValidBarcodeReturnsFalseWhenItsClassificationIsEmpty()
        {
            Assert.False(Holding.IsBarcodeValid(":1"));
        }

        [Fact]
        public void IsValidBarcodeReturnsTrueWhenFormattedCorrectly()
        {
            Assert.True(Holding.IsBarcodeValid("ABC:1"));
        }

        [Fact]
        public void GenBarcode()
        {
            Assert.Equal(ExpectedBarcode, Holding.GenerateBarcode("QA234", 3));
        }

        [Fact]
        public void ClassificationFromBarcode()
        {
            try
            {
                Assert.Equal("QA234", Holding.ClassificationFromBarcode(ExpectedBarcode));
            }
            catch (FormatException)
            {
                Assert.True(false, "should not thro fmt except");
            }
        }

        [Fact]
        public void BarcodePts()
        {
            Assert.Equal(("QA234", 42), Holding.BarcodeParts("QA234:42"));
        }

        [Fact]
        public void ParsesCopyNoFromBarcode()
        {
            try
            {
                Assert.Equal(3, Holding.CopyNumberFromBarcode(ExpectedBarcode));
            }
            catch (FormatException)
            {
                Assert.False(true, "test threw format exception");
            }
        }

        [Fact]
        public void CopyNumberFromBarcodeThrowsWhenNoColonExists()
        {
            Assert.Throws<FormatException>(() => Holding.CopyNumberFromBarcode("QA234"));
        }

        [Fact]
        public void BookIsCheckedOutAfterCheckout()
        {
            var holding = new Holding { Classification = "", CopyNumber = 1, BranchId = 1 };
            var checkedOutTime = DateTime.Now;

            var policy = CheckoutPolicies.BookCheckoutPolicy;
            holding.CheckOut(checkedOutTime, PatronId, policy);

            Assert.True(holding.IsCheckedOut);
        }

        [Fact]
        public void Co()
        {
            var holding = new Holding {Classification = "", CopyNumber = 1, BranchId = 1};
            var checkedOutTime = DateTime.Now;

            var policy = CheckoutPolicies.BookCheckoutPolicy;
            holding.CheckOut(checkedOutTime, PatronId, policy);

            Assert.True(holding.IsCheckedOut);

            Assert.Equal(policy.Id, holding.CheckoutPolicy.Id);
            Assert.Equal(PatronId, holding.HeldByPatronId);

            var dueDate = checkedOutTime.AddDays(policy.MaximumCheckoutDays());
            Assert.Equal(dueDate, holding.DueDate);

            Assert.Equal(Branch.CheckedOutId, holding.BranchId);
            
            // checking in
            var tomorrow = DateTime.Now.AddDays(1);
            const int newBranchId = 2;
            holding.CheckIn(tomorrow, newBranchId);
            Assert.False(holding.IsCheckedOut);
            Assert.Equal(Holding.NoPatron, holding.HeldByPatronId);
            Assert.Null(holding.CheckOutTimestamp);
            Assert.Equal(newBranchId, holding.BranchId);
            // day after now
            Assert.Equal(tomorrow, holding.LastCheckedIn);
        }

        [Fact]
        public void CheckInAnswersZeroDaysLateWhenReturnedOnDueDate()
        {
            var holding = new Holding {Classification = "X", BranchId = 1, CopyNumber = 1};
            holding.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            var dueDate = holding.DueDate.Value;
            int brId = 2;

            holding.CheckIn(dueDate, brId);
            Assert.Equal(0, holding.DaysLate());
        }

        [Fact]
        public void DaysLateCalculatedWhenReturnedAfterDueDate()
        {
            var holding = new Holding {Classification = "X", BranchId = 1, CopyNumber = 1};
            holding.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            var date = holding.DueDate.Value.AddDays(2);
            var branchId = 2;

            holding.CheckIn(date, branchId);
            Assert.Equal(2, holding.DaysLate());
        }

        [Fact]
        public void CheckInAnswersZeroDaysLateWhenReturnedBeforeDueDate()
        {
            var holding = new Holding {Classification = "X", BranchId = 1, CopyNumber = 1};
            holding.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            var date = holding.DueDate.Value.AddDays(-1);
            int branchId = 2;

            holding.CheckIn(date, branchId);
            Assert.Equal(0, holding.DaysLate());
        }
    }
}