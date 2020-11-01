using System;
using LibraryNet2020.Util;
using Xunit;

namespace LibraryTest.Util
{
    public class TimeServiceTest
    {
        [Fact]
        public void ReturnsCurrentTimeIfNextTimeNotSpecified()
        {
            var retrievedTime = TimeService.Now;
            
            Assert.True((retrievedTime - DateTime.Now).Seconds < 2);
        }
        
        [Fact]
        public void ReturnsSpecifiedTimeWhenAskedForNow()
        {
            var time = new DateTime(2022, 3, 15);
            TimeService.NextTime = time;
            
            Assert.Equal(time, TimeService.Now);
        }
        
        [Fact]
        public void OnlyReturnsOneSpecifiedTime()
        {
            var time = new DateTime(2022, 3, 15);
            TimeService.NextTime = time;
            Assert.Equal(time, TimeService.Now);

            var retrievedTime = TimeService.Now;
            
            Assert.True((retrievedTime - DateTime.Now).Seconds < 2);
        }
    }
}