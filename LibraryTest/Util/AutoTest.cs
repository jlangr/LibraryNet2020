using System;
using System.Collections.Generic;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Util
{
    public class Auto
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

    public class AutoTest
    {
        [Fact]
        public void IdlesEngineWhenStarted()
        {
            var auto = new Auto();
            auto.DepressBrake();

            auto.PressStartButton();

            Assert.InRange(auto.RPM, 950, 1100);
        }
    }

    public class MiscTest
    {
        [Fact]
        public void Assertions()
        {
            var condition = false;
            var text = "something";
            var obj = new Auto();
            var tokens = new List<string> {"public", "void", "return"};
            var zero = 8 - 8;
            var someEnumerable = new List<string>();

            Assert.False(condition);
            Assert.Equal("something", text);
            Assert.NotEqual("something else", text);
            Assert.Contains("tech", "technology"); // also DoesNotContain
            Assert.Matches(".*thing$", "something");
            Assert.Throws<DivideByZeroException>(() => 4 / zero);
            Assert.Empty(someEnumerable); // also NotEmpty
            Assert.IsType<Auto>(obj);
            Assert.Collection(new List<int> {2, 4},
                n => Assert.Equal(2, n),
                n => Assert.Equal(4, n)
            );
            Assert.All(new List<string> {"a", "ab", "abc"},
                s => s.StartsWith("a"));
        }

        class Something
        {
            public void MoreSetup()
            {
            }

            public void EvenMoreSetup()
            {
            }

            public void DoStuff()
            {
            }

            public bool SomeProperty { get; set; } = true;
        }

        [Fact]
        public void AAAIsAVisualMnemonic()
        {
            // arrange
            var x = new Something();
            x.MoreSetup();
            x.EvenMoreSetup();

            // act
            x.DoStuff();

            // assert
            Assert.True(x.SomeProperty);
        }

        [Fact]
        public void NoCommentsPlease()
        {
            var x = new Something();
            x.MoreSetup();
            x.EvenMoreSetup();

            x.DoStuff();

            Assert.True(x.SomeProperty);
        }

        [Fact]
        public void MoqSimpleStub()
        {
            var mock = new Moq.Mock<IList<string>>();
            mock.Setup(l => l.Count)
                .Returns(42);
            IList<string> list = mock.Object;

            Assert.Equal(42, list.Count);
            
            mock.Setup(l => l[15])
                .Returns("1500");
        }
    }
}