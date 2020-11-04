using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Util
{
    public class BasicsTest
    {
        [Fact(Skip="unskip when ready to work")]
        public void SupportsBasicMath()
        {
            Assert.Equal(ReplaceThisWithWhatTheRealAnswerShouldBe, 4 * 8);
        }

        [Fact(Skip="unskip when ready to work")]
        public void AppendsItemToListViaAdd()
        {
            var numbers = new List<int> { 12, 1, 1, 1, 2, 1, 3 };
            
            numbers.Add(1);  // (yes this could be part of the numbers initialization)

            Assert.Equal(new List<int> { ReplaceThisWithWhatTheRealAnswerShouldBe }, numbers);
        }

        [Fact(DisplayName = "double each element in list of numbers", Skip = "unskip when ready to work")]
        public void DoublesEachElementInListOfNumbers()
        {
            var numbers = new List<int> { 2, 5, 10, 105 };

            var result = ReplaceThisWithAppropriateCode();
            
            Assert.Equal(new List<int> { 4, 10, 20, 210 }, result);
        }

        [Fact(Skip="unskip when ready to work")]
        public void HandlesInterestingFloatPointNumericResults()
        {
            var result = 0.1 + 0.2;
            
            // fix this so it works
            Assert.Equal(0.3, result);
        }
        
        #region nothing to change here
        private readonly int ReplaceThisWithWhatTheRealAnswerShouldBe = -1;
        
        private static object ReplaceThisWithAppropriateCode()
        {
            // no don't put the code in here, just delete the call to this method
            return null;
        }
        #endregion
    }
}

