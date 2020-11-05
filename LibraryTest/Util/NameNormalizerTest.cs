using System;
using LibraryNet2020.Util;
using Xunit;
using Assert = Xunit.Assert;

// 1. Un-skip the next commented-out test method.
// 2. Run all tests in the project.
// 3. Did the current test fail?
//     If not: Stop. You built too much code in a prior step. Undo work for prior tests and try again.
// 4. Make sure you are clear on why the test failed.
// 5. Write only enough code to make that failing test pass (and not break any other tests).
//      Is there a simpler way to get that test to pass, without any extra behaviors?
//      Assume that you need only support the behaviors described in tests so far.
// 6. Is there another assertion that is commented out? Uncomment it. It should fail. Make it pass.
// 7. Return to step 1.
// Out of tests? Can you think of tests you might also want to write? Go for it!

// Coding hint:
//   s.Count(c => c == ',') will count all the comma characters in string s.

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

        [Fact(Skip="unskip when ready to work")]
        public void TrimsWhitespace()
        {
            Assert.Equal("Boi, Big", normalizer.Normalize("  Big Boi   "));
        }

        [Fact(Skip="unskip when ready to work")]
        public void InitializesMiddleName()
        {
            Assert.Equal("Thoreau, Henry D.", normalizer.Normalize("Henry David Thoreau"));
        }

        [Fact(Skip="unskip when ready to work")]
        public void DoesNotInitializeOneLetterMiddleName()
        {
            Assert.Equal("Truman, Harry S", normalizer.Normalize("Harry S Truman"));
        }

        [Fact(Skip="unskip when ready to work")]
        public void InitializesEachOfMultipleMiddleNames()
        {
            Assert.Equal("Louis-Dreyfus, Julia S. E.", normalizer.Normalize("Julia Scarlett Elizabeth Louis-Dreyfus"));
        }

        [Fact(Skip="unskip when ready to work")]
        public void AppendsSuffixesToEnd()
        {
            Assert.Equal("King, Martin L., Jr.", normalizer.Normalize("Martin Luther King, Jr."));
        }

        [Fact(Skip="unskip when ready to work")]
        public void ThrowsWhenNameContainsTwoCommas()
        {
            var exception = Assert.Throws<ArgumentException>(() => normalizer.Normalize("Thurston, Howell, III"));
            Assert.Equal("name can have at most one comma", exception.Message);
        }
    }
}