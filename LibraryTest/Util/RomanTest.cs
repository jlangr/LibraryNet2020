using System.Linq;
using Xunit;

namespace LibraryTest.Util
{
    public class RomanTest
    {
        readonly (string, int)[] Conversions =
        {
            ("C", 100),
            ("X", 10),
            ("V", 5),
            ("IV", 4),
            ("I", 1)
        };

        string Repeat(string s, int times) => string.Concat(Enumerable.Repeat(s, times));

        string Convert(int arabic)
        {
            var s = "";
            foreach (var (romanDigit, arabicDigit) in Conversions)
            {
                var romanDigitsNeeded = arabic / arabicDigit;
                s += Repeat(romanDigit, romanDigitsNeeded);
                arabic -= arabicDigit * romanDigitsNeeded;
            }

            return s;
        }

        [Fact]
        public void Converts()
        {
            Assert.Equal("I", Convert(1));
            Assert.Equal("II", Convert(2));
            Assert.Equal("III", Convert(3));
            Assert.Equal("IV", Convert(4));
            Assert.Equal("V", Convert(5));
            Assert.Equal("X", Convert(10));
            Assert.Equal("XI", Convert(11));
            Assert.Equal("XX", Convert(20));
            Assert.Equal("CCC", Convert(300));
        }
    }
}