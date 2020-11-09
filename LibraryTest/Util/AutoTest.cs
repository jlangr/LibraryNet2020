using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Util
{
    class DiagnosticDevice {
        public string Results()
        {
            return "diagnostics: ";
        }

        public void Start()
        {
            throw new DiagnosticsException("unable to open device");
        }

        public void Run()
        {
        }

        public object System { get; set; } = "engine";
    }

    internal class DiagnosticsException : Exception
    {
        public DiagnosticsException(string message) : base(message)
        {
        }
    }

    public class Auto
    {
        public Auto()
        {
            RPM = 950;
        }
        
        private DiagnosticDevice DiagnosticDevice { get; set; }

        private const string Module = "Auto";

        public void DepressBrake()
        {
        }

        public void PressStartButton()
        {
        }

        public void Log(string message)
        {
            // Console.WriteLine(message);
        }
        
        private string System { get; set; }

        public void RunDiagnostics()
        {
            try
            {
                DiagnosticDevice.Start();
                DiagnosticDevice.Run();
                Console.WriteLine(DiagnosticDevice.Results());
            }
            catch(DiagnosticsException e) { 
                var errMsg = $"{DateTime.Now}: {System}-{Module} " +
                             $"{LogLevel.Error} {e.Message.Trunc(80)}";
                Log(errMsg);
                throw new ApplicationException(errMsg, e);
            } 
        }
        
        public void RunDiagnostics2()
        {
            try
            {
                DiagnosticDevice.Start();
                DiagnosticDevice.Run();
                Console.WriteLine(DiagnosticDevice.Results());
            }
            catch(DiagnosticsException e) { 
                var errMsg = FormattedErrorMessage(e, Module, System);
                Log(errMsg);
                throw new ApplicationException(errMsg, e);
            } 
        }

        private string FormattedErrorMessage(DiagnosticsException e, string module, string system)
        {
            return $"{DateTime.Now}: {system}-{module} {LogLevel.Error} {e.Message.Trunc(80)}";
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

        [Fact]
        public void moq()
        {
            var mock = new Mock<IDictionary<object, string>>();
            var dictionary = mock.Object;

            mock.Setup(d => d[It.IsAny<string>()])
                .Returns("a fish");

            Assert.Equal("a fish", dictionary["smelt"]);
            Assert.Equal("a fish", dictionary["trout"]);
            Assert.NotEqual("a fish", dictionary[42]);

            mock
                .Setup(d => d[It.Is<string>(s => s.Last() == 's')])
                .Returns("maybe plural");
            mock
                .Setup(d => d[It.Is<string>(s => s.Last() != 's')])
                .Returns("maybe singular");

            Assert.Equal("maybe plural", dictionary["dogs"]);
            Assert.Equal("maybe singular", dictionary["trout"]);
        }

        private IList<string> list;

        public MiscTest()
        {
            var mock = new Mock<IList<string>>();
            list = mock.Object;
        }

        [Fact]
        public void MoqRetrieveMock()
        {
            Mock.Get(list).Setup(l => l.Count)
                .Returns(42);

            Assert.Equal(42, list.Count);
        }

        public interface Cache
        {
            string LookUp(string key);
        }

        [Fact]
        public void MockOfSyntax()
        {
            var cache =
                Mock.Of<Cache>(c => c.LookUp("smelt") == "a fish");

            Assert.Equal(cache.LookUp("smelt"), "a fish");
        }

        public class Store
        {
            public virtual string LookUp(string key)
            {
                throw new Exception("shouldn't get here");
            }
        }

        [Fact]
        public void VirtualAndNot()
        {
            var store =
                Mock.Of<Store>(c => c.LookUp("smelt") == "a fish");

            Assert.Equal(store.LookUp("smelt"), "a fish");
        }


        private static ulong GCD(ulong x, ulong y)
        {
            while (x != 0 && y != 0)
                if (x > y)
                    x %= y;
                else
                    y %= x;
            return x | y;
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 19, 1)]
        [InlineData(10, 20, 10)]
        [InlineData(55, 88, 11)]
        public void PrimeFactors(ulong x, ulong y, ulong expected)
        {
            Assert.Equal(expected, GCD(x, y));
        }
    }
}