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

        public class WithNoHoldingYetTest : HoldingTest
        {
            [Fact]
            public void IsCheckedOutWhenNoBranchIsSet()
            {
                var holding = new Holding {BranchId = Branch.CheckedOutId};
                Assert.True(holding.IsCheckedOut);
                Assert.False(holding.IsAvailable);
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
                Assert.Equal("QA234", Holding.ClassificationFromBarcode(ExpectedBarcode));
            }

            [Fact]
            public void BarcodePts()
            {
                Assert.Equal(("QA234", 42), Holding.BarcodeParts("QA234:42"));
            }

            [Fact]
            public void ParsesCopyNoFromBarcode()
            {
                Assert.Equal(3, Holding.CopyNumberFromBarcode(ExpectedBarcode));
            }

            [Fact]
            public void CopyNumberFromBarcodeThrowsWhenNoColonExists()
            {
                Assert.Throws<FormatException>(() => Holding.CopyNumberFromBarcode("QA234"));
            }
        }

        public class WithHoldingAtBranch1Test : HoldingTest
        {
            private readonly Holding holdingAtBranch1 = new Holding {BranchId = BranchId1};

            [Fact]
            public void IsAvailableOutWhenBranchIsSet()
            {
                Assert.False(holdingAtBranch1.IsCheckedOut);
                Assert.True(holdingAtBranch1.IsAvailable);
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
        }

        public class CheckIn_GivenCheckedOutHoldingTest : HoldingTest
        {
            private readonly Holding holdingCheckedOut = new Holding {BranchId = BranchId1};

            public CheckIn_GivenCheckedOutHoldingTest()
            {
                holdingCheckedOut.CheckOut(today, PatronId, CheckoutPolicies.BookCheckoutPolicy);
            }

            [Fact]
            public void UpdatesLastCheckedInDate()
            {
                holdingCheckedOut.CheckIn(tomorrow, BranchId2);

                Assert.Equal(tomorrow, holdingCheckedOut.LastCheckedIn);
            }

            [Fact]
            public void UpdatesBranchId()
            {
                holdingCheckedOut.CheckIn(tomorrow, BranchId2);

                Assert.Equal(BranchId2, holdingCheckedOut.BranchId);
            }

            [Fact]
            public void ClearsCheckOutTimestamp()
            {
                holdingCheckedOut.CheckIn(tomorrow, BranchId2);

                Assert.Null(holdingCheckedOut.CheckOutTimestamp);
            }

            [Fact]
            public void UpdatesAvailability()
            {
                holdingCheckedOut.CheckIn(tomorrow, BranchId2);

                Assert.False(holdingCheckedOut.IsCheckedOut);
                Assert.True(holdingCheckedOut.IsAvailable);
            }

            [Fact]
            public void ClearsPatronId()
            {
                holdingCheckedOut.CheckIn(tomorrow, BranchId2);

                Assert.Equal(Holding.NoPatron, holdingCheckedOut.HeldByPatronId);
            }

            [Fact]
            public void NotLateWhenReturnedOnDueDate()
            {
                holdingCheckedOut.CheckIn(holdingCheckedOut.DueDate.Value, BranchId2);

                Assert.Equal(0, holdingCheckedOut.DaysLate());
            }

            [Fact]
            public void LateUpdatesDaysLate()
            {
                holdingCheckedOut.CheckIn(holdingCheckedOut.DueDate.Value.AddDays(2), BranchId2);

                Assert.Equal(2, holdingCheckedOut.DaysLate());
            }

            [Fact]
            public void NotLateWhenReturnedBeforeDueDate()
            {
                holdingCheckedOut.CheckIn(holdingCheckedOut.DueDate.Value.AddDays(-1), BranchId2);

                Assert.Equal(0, holdingCheckedOut.DaysLate());
            }
        }
    }
}