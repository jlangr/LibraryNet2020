using LibraryNet2020.Util;
using Xunit;
using Assert = Xunit.Assert;

//Implementing the Soundex Algorithm--Test-First
//
//Per Wikipedia, Soundex is a phonetic algorithm for indexing names
//by sound, as pronounced in English. The goal is for homophones to
//be encoded to the same representation so that they can be matched
//despite minor differences in spelling.
//
//Each [Fact] method in SoundexTest.java describes a small increment
//of behavior needed to build the Soundex algorithm.
//
// First: Remove the multiline comments around all the tests.
//
//For each test from top to bottom:
//- Remove the Skip annotation attribute.
//- Run all tests. Ensure the newly-unignored (current) test fails,
//  and that all other previously passing tests still pass.
//- Write code in Soundex.java to get the current test to pass.
//  Build no more implementation than required.
//
//You'll find a couple very useful helper methods in Soundex.java.
//Use these so you can focus on incrementing the core algorithm.
//
//If you un-ignore a test and it immediately passes,
//you wrote too much code to get a prior test to pass.
//Return to the prior step and find a way to get prior tests
//passing with the minimal code needed.
//
//Read additional comments in each test and follow any additional rules specified.
//
//The rules for Soundex, per Wikipedia (http://en.wikipedia.org/wiki/Soundex):
//1. Retain the first letter of the name and drop all other occurrences of a,e,i,o,u,y,h,w.
//2. Replace consonants with digits as follows (after the first letter):
//    b, f, p, v => 1
//    c, g, j, k, q, s, x, z => 2
//    d, t => 3
//    l => 4
//    m, n => 5
//    r => 6
//3. Two adjacent letters (in the original name) with the same number are coded as a single
//   number; also two letters with the same number separated by 'h' or 'w' are coded as a
//   single number, whereas such letters separated by a vowel are coded twice. This rule
//   also applies to the first letter.
//4. Continue until you have one letter and three numbers. If you run out of letters, fill
//   in 0s until there are three numbers.

namespace LibraryCoreTests.Util
{
    public class SoundexTest
    {
        private Soundex soundex;

        public SoundexTest()
        {
            soundex = new Soundex();
        }

        [Fact(Skip="until you're ready to uncomment this test and get it to pass!")]
        public void RetainsSoleLetterOfOneLetterWord()
        {
            Assert.Equal("A000", soundex.Encode("A"));
        }

        [Fact(Skip="until ready")]
        public void ReplacesConsonantsWithAppropriateDigits()
        {
            Assert.Equal("A100", soundex.Encode("Ab"));
            // Uncomment each subsequent assertion, and get to pass, one at a time:
            // Assert.Equal("B234", soundex.encode("Bcdl"));
            // Assert.Equal("A256", soundex.encode("Ajmr"));

            // Prepare to discuss: Should we have multiple assertions in one test?
        }

        [Fact(Skip="until ready")]
        public void LimitsLengthToFourCharacters()
        {
            Assert.Equal("D123", soundex.Encode("Dbcdlmr"));
        }

        [Fact(Skip="until ready")]
        public void SkipsVowelLikeLetters()
        {
            Assert.Equal("F232", soundex.Encode("Faeiouhycts"));
        }

        [Fact(Skip="until ready")]
        public void CombinesDuplicateEncodingsAsSingleNumber()
        {
            // Prepare to discuss:
            // - What is the value of these three preconditions?
            // - What is the cost (tradeoff)? How do you feel about the design choice?
            //Assert.Equal(soundex.toDigit('f'), soundex.toDigit('b'));
            //Assert.Equal(soundex.toDigit('g'), soundex.toDigit('c'));
            //Assert.Equal(soundex.toDigit('t'), soundex.toDigit('d'));

            Assert.Equal("G123", soundex.Encode("Gbfcgdt"));
        }

        [Fact(Skip="until ready")]
        public void UppercasesFirstLetter()
        {
            Assert.Equal("A123", soundex.Encode("abcd"));
        }

        [Fact(Skip="until ready")]
        public void SkipsVowelLikeLettersRegardlessOfCase()
        {
            Assert.Equal("F232", soundex.Encode("FcAEIOUHYts"));
        }

        [Fact(Skip="until ready")]
        public void ReplacesConsonantsWithAppropriateDigitsRegardlessOfCase()
        {
            Assert.Equal("B234", soundex.Encode("BCDL"));
        }

        [Fact(Skip="until ready")]
        public void CombinesDuplicateEncodingsWhenSecondLetterDuplicatesFirst()
        {
            Assert.Equal("B230", soundex.Encode("Bbcd"));
        }

        [Fact(Skip="until ready")]
        public void DoesNotCombineDuplicateEncodingsSeparatedByVowels()
        {
            Assert.Equal("J110", soundex.Encode("Jbobby"));
        }
        // Congratulations if you made it this far!
        // Prepare to discuss:
        // - What other tests are missing?
        // - What were the costs and benefits of building Soundex incrementally?
    }
}
