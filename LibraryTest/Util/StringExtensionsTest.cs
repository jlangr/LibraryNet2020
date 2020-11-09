using Xunit;

namespace LibraryTest.Util
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void TruncateReturnsIdentityIfNullOrEmpty(string input)
        {
            Assert.Equal(input, input.Trunc(10));
        }

        [Fact]
        public void TruncateReturnsWholeStringIfUnderMaxLength()
        {
            Assert.Equal("short string", "short string".Trunc(50));
        }

        [Fact]
        public void TruncateReturnsTruncatedStringWhenOverMaxLength()
        {
            Assert.Equal("12345", "1234567890".Trunc(5));
        }
    }
}