using System;
using LibraryNet2020.Util;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryCoreTests.Util
{
    public class NameNormalizerTest
    {
        private NameNormalizer normalizer;

        public NameNormalizerTest()
        {
            normalizer = new NameNormalizer();
        }

        [Fact]
        public void ReturnsEmptyStringWhenEmpty()
        {
            Assert.Equal("", normalizer.Normalize(""));
        }

        [Fact]
        public void ReturnsSingleWordName()
        {
            Assert.Equal("Plato", normalizer.Normalize("Plato"));
        }

        [Fact]
        public void ReturnsLastFirstWhenFirstLastProvided()
        {
            Assert.Equal("Murakami, Haruki", normalizer.Normalize("Haruki Murakami"));
        }

        [Fact]
        public void TrimsWhitespace()
        {
            Assert.Equal("Boi, Big", normalizer.Normalize("  Big Boi   "));
        }

        [Fact]
        public void InitializesMiddleName()
        {
            Assert.Equal("Thoreau, Henry D.", normalizer.Normalize("Henry David Thoreau"));
        }

        [Fact]
        public void DoesNotInitializeOneLetterMiddleName()
        {
            Assert.Equal("Truman, Harry S", normalizer.Normalize("Harry S Truman"));
        }

        [Fact]
        public void InitializesEachOfMultipleMiddleNames()
        {
            Assert.Equal("Louis-Dreyfus, Julia S. E.", normalizer.Normalize("Julia Scarlett Elizabeth Louis-Dreyfus"));
        }

        [Fact]
        public void AppendsSuffixesToEnd()
        {
            Assert.Equal("King, Martin L., Jr.", normalizer.Normalize("Martin Luther King, Jr."));
            Assert.Equal("Langr, Jeff, Esq.", normalizer.Normalize("Jeff Langr, Esq."));
            Assert.Equal("Madonna, Jr.", normalizer.Normalize("Madonna, Jr."));
        }

        [Fact]
        public void TrimsAtSuffixEnd()
        {
            Assert.Equal("Madonna, Jr.", normalizer.Normalize("Madonna, Jr.   "));
        }

        [Fact]
        public void ThrowsWhenNameContainsMoreThanComma()
        {
            var exception = 
                Assert.Throws<ArgumentException>(() => normalizer.Normalize("Thurston, Howell, III"));
            Assert.Equal("name can have at most one comma", exception.Message);
        }
    }
}