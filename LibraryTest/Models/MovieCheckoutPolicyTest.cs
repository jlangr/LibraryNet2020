using LibraryNet2020.NonPersistentModels;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Models
{
    public class MovieCheckoutPolicyTest
    {
        private CheckoutPolicy policy;

        public MovieCheckoutPolicyTest()
        {
            policy = new MovieCheckoutPolicy();
        }

        [Fact]
        public void DailyAccumulatingFine()
        {
            var daysLate = 1;
            Assert.Equal(MovieCheckoutPolicy.PenaltyAmount + MovieCheckoutPolicy.DailyFineBasis * 1, policy.FineAmount(daysLate++));
            Assert.Equal(MovieCheckoutPolicy.PenaltyAmount + MovieCheckoutPolicy.DailyFineBasis * 2, policy.FineAmount(daysLate++));
            Assert.Equal(MovieCheckoutPolicy.PenaltyAmount + MovieCheckoutPolicy.DailyFineBasis * 3, policy.FineAmount(daysLate));
        }
    }
}
