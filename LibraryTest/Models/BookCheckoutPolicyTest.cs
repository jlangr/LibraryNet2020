using System;
using LibraryNet2020.NonPersistentModels;
using Xunit;

namespace LibraryTest.Models
{
    public class BookCheckoutPolicyTest
    {
        private CheckoutPolicy policy;

        public BookCheckoutPolicyTest()
        {
            policy = new BookCheckoutPolicy();
        }

        // TODO Use abstract test here and for next test!
        [Fact]
        public void NoDaysLateIfReturnedOnTime()
        {
            var checkoutDate = DateTime.Now;
            
            var checkinDate = checkoutDate.AddDays(policy.MaximumCheckoutDays());
            
            Assert.Equal(0, policy.DaysLate(checkoutDate, checkinDate));
        }

        [Fact]
        public void OneDayLateWhenReturnedDayAfterDue()
        {
            var checkoutDate = DateTime.Now;
            
            var checkinDate = checkoutDate.AddDays(policy.MaximumCheckoutDays() + 1);
            
            Assert.Equal(1, policy.DaysLate(checkoutDate, checkinDate));
        }

        [Fact]
        public void FineIsDaysLateTimesBasis()
        {
            var daysLate = 1;
            
            Assert.Equal(BookCheckoutPolicy.DailyFineBasis * 1, policy.FineAmount(daysLate++));
            Assert.Equal(BookCheckoutPolicy.DailyFineBasis * 2, policy.FineAmount(daysLate++));
            Assert.Equal(BookCheckoutPolicy.DailyFineBasis * 3, policy.FineAmount(daysLate));
        }
    }
}
