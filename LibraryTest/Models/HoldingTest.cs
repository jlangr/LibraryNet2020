using System;
using LibraryNet2020.Models;
using LibraryNet2020.NonPersistentModels;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Models
{
    public class HoldingTest
    {
        const int PatronId = 101;
        const string ExpectedBarcode = "QA234:3";
        const int BranchId1 = 1;
        const int BranchId2 = 2;
        private readonly DateTime today = DateTime.Now;
        private readonly DateTime tomorrow = DateTime.Now.AddDays(1);
        private readonly Holding holdingAtBranch1 = new Holding { BranchId = BranchId1 };

        [Fact]
        public void IsCheckedOutWhenBranchIsTheCheckedOutBranch()
        {
            var holding = new Holding {BranchId = Branch.CheckedOutId};
            Assert.True(holding.IsCheckedOut);
            Assert.False(holding.IsAvailable);
        }

        [Fact]
        public void IsAvailableOutWhenBranchIsSet()
        {
            Assert.False(holdingAtBranch1.IsCheckedOut);
            Assert.True(holdingAtBranch1.IsAvailable);
        }

        [Fact]
        public void CanCreateWithCommonArguments()
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
        public void CheckOutSetsHoldingToCheckedOutBranch()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            Assert.Equal(Branch.CheckedOutId, holdingAtBranch1.BranchId);
        }

        [Fact]
        public void CheckOutUpdatedWithPatronId()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            Assert.Equal(PatronId, holdingAtBranch1.HeldByPatronId);
        }

        [Fact]
        public void CheckOutUpdatesDueDateDetails()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            Assert.Equal(CheckoutPolicies.BookCheckoutPolicy.Id, holdingAtBranch1.CheckoutPolicy.Id);
            Assert.Equal(today.AddDays(CheckoutPolicies.BookCheckoutPolicy.MaximumCheckoutDays()),
                holdingAtBranch1.DueDate);
        }

        [Fact]
        public void CheckOutUpdatesAvailabilityFlags()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            Assert.True(holdingAtBranch1.IsCheckedOut);
            Assert.False(holdingAtBranch1.IsAvailable);
        }

        [Fact]
        public void CheckOutSetsCheckOutTimestamp()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            Assert.Equal(today, holdingAtBranch1.CheckOutTimestamp);
        }

        [Fact]
        public void CheckInUpdatesLastCheckedInDate()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(tomorrow, BranchId2);

            Assert.Equal(tomorrow, holdingAtBranch1.LastCheckedIn);
        }
        
        [Fact]
        public void CheckInUpdatesBranchId()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(tomorrow, BranchId2);

            Assert.Equal(BranchId2, holdingAtBranch1.BranchId);
        }
        
        [Fact]
        public void CheckInClearsCheckOutTimestamp()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(tomorrow, BranchId2);

            Assert.Null(holdingAtBranch1.CheckOutTimestamp);
        }

        [Fact]
        public void CheckInUpdatesAvailability()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(tomorrow, BranchId2);

            Assert.False(holdingAtBranch1.IsCheckedOut);
            Assert.True(holdingAtBranch1.IsAvailable);
        }

        [Fact]
        public void CheckInClearsPatronId()
        {
            holdingAtBranch1.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(tomorrow, BranchId2);

            Assert.Equal(Holding.NoPatron, holdingAtBranch1.HeldByPatronId);
        }

        [Fact]
        public void CheckInNotLateWhenReturnedOnDueDate()
        {
            holdingAtBranch1.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(holdingAtBranch1.DueDate.Value, BranchId2);
            
            Assert.Equal(0, holdingAtBranch1.DaysLate());
        }

        [Fact]
        public void CheckInLateUpdatesDaysLate()
        {
            holdingAtBranch1.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(holdingAtBranch1.DueDate.Value.AddDays(2), BranchId2);
            
            Assert.Equal(2, holdingAtBranch1.DaysLate());
        }

        [Fact]
        public void CheckInNotLateWhenReturnedBeforeDueDate()
        {
            holdingAtBranch1.CheckOut(DateTime.Now, PatronId, CheckoutPolicies.BookCheckoutPolicy);

            holdingAtBranch1.CheckIn(holdingAtBranch1.DueDate.Value.AddDays(-1), BranchId2);
            
            Assert.Equal(0, holdingAtBranch1.DaysLate());
        }
    }
}