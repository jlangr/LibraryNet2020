using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Util
{
    public class AutoTest
    {
        class Auto
        {
            public Auto()
            {
                RPM = 950;
            }

            public void DepressBrake()
            {
            }

            public void PressStartButton()
            {
            }

            public int RPM { get; set; }
        }

        [Fact]
        public void IdlesEngineWhenStarted()
        {
            var auto = new Auto();
            auto.DepressBrake();

            auto.PressStartButton();

            Assert.InRange(auto.RPM, 950, 1100);
        }
    }
}