using System;
using LibraryNet2020.NonPersistentModels;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Models
{
    public class CheckoutPolicyTest
    {
        [Fact]
        public void NotLateIfReturnedWithinMaxDays()
        {
            const int maxCheckoutDays = 5;

            var checkoutDate = DateTime.Now;
            var checkinDate = DateTime.Now.AddDays(maxCheckoutDays);

            Assert.Equal(0, new StubCheckoutPolicy().DaysLate(checkoutDate, checkinDate, maxCheckoutDays));
        }

        [Fact]
        public void OneDayLate()
        {
            const int maxCheckoutDays = 5;

            var checkoutDate = DateTime.Now;
            var checkinDate = DateTime.Now.AddDays(maxCheckoutDays + 1);

            Assert.Equal(1, new StubCheckoutPolicy().DaysLate(checkoutDate, checkinDate, maxCheckoutDays));
        }

        [Fact]
        public void ACoupleYearsLate()
        {
            const int maxCheckoutDays = 2;
            var checkoutDate = new DateTime(2017, 1, 1);
            var checkinDate = new DateTime(2019, 1, 1);
            Assert.Equal(365 * 2 - 2,
                new StubCheckoutPolicy().DaysLate(checkoutDate, checkinDate, maxCheckoutDays));
        }

        [Fact]
        public void CalculatesFineFromDaysAndPeriod()
        {
            var policy = new StubCheckoutPolicy();

            const int daysLate = 2;

            var checkoutDate = DateTime.Now;
            var checkinDate = DateTime.Now.AddDays(policy.MaximumCheckoutDays() + daysLate);

            Assert.Equal(StubCheckoutPolicy.FixedFine, policy.FineAmount(checkoutDate, checkinDate));
            Assert.Equal(daysLate, StubCheckoutPolicy.LastDaysLate);
        }
    }

    class StubCheckoutPolicy : CheckoutPolicy
    {
        public const decimal FixedFine = 100;
        public static int LastDaysLate;
        
        public override int MaximumCheckoutDays()
        {
            return 10;
        }

        public override decimal FineAmount(int daysLate)
        {
            LastDaysLate = daysLate;
            return FixedFine;
        }

        public override string Id { get; }
    }
}
