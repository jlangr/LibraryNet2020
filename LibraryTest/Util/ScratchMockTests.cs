using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace LibraryTest.Util
{
    public class DictionarySvc
    {
        public Auditor EventAuditor { get; set; }

        public virtual string LookUp(string key)
        {
            throw new Exception();
        }
        public virtual string StringStuff(string input)
        {
            throw new Exception();
        }
        public virtual void Add(string word, string definition)
        {
            EventAuditor.Initialize();
            EventAuditor.Log($"adding {word}:{definition}");
        }
    }

    public interface IEmail
    {
        void Send(string recipient, string sender, string title, string content);
    }

    public class Auditor
    {
        public virtual void Initialize() { }
        public virtual void Log(string message) { throw new Exception("LOGGER DOWN"); }
    }

    public class Client
    {
        public string GetDefinition(string word) { return CreateDict().LookUp(word); }
        public virtual DictionarySvc CreateDict() { return new DictionarySvc(); }
    }

    public class ScratchMoqTest
    {
        private DictionarySvc dictionaryService;

        class TestClient : Client
        {
            public DictionarySvc Dict { get; set; }
            public override DictionarySvc CreateDict() { return Dict; }
        }

        public ScratchMoqTest()
        {
            dictionaryService = new DictionarySvc();
        }

        [Fact]
        public void LogsAuditRecordOnAdd()
        {
            var auditorSpy = new Mock<Auditor>();
            dictionaryService.EventAuditor = auditorSpy.Object;

            dictionaryService.Add("dog", "a canine");

            auditorSpy.Verify(auditor => auditor.Log("adding dog:a canine"));
        }
        [Fact]
        public void LogsAuditRecordOnAdd_Strict()
        {
            var auditorSpy = new Mock<Auditor>(MockBehavior.Strict);
            dictionaryService.EventAuditor = auditorSpy.Object;
            auditorSpy.Setup(auditor => auditor.Initialize());
            auditorSpy.Setup(auditor => auditor.Log("adding dog:a canine"));

            dictionaryService.Add("dog", "a canine");

            //auditorSpy.Verify(auditor => auditor.Log("adding dog:a canine"));
        }
        [Fact]
        public void PropertyOverride()
        {

            TestClient c = new TestClient();
            var mock = new Mock<DictionarySvc>();
            mock.Setup(d => d.LookUp("smelt")).Returns("hooo");
            c.Dict = mock.Object;
            Assert.Equal("hooo", c.GetDefinition("smelt"));

        }
        [Fact]
        public void X()
        {
            var mock = new Mock<IList<string>>();
            mock.Setup(l => l.Count).Returns(42);
            IList<string> list = mock.Object;
            Assert.Equal(42, list.Count);
        }

        [Fact]
        public void Args()
        {
            var dictionary = Mock.Of<DictionarySvc>(s => s.LookUp("smelt") == "a fish");

            Assert.Equal("a fish", dictionary.LookUp("smelt"));
        }

        [Fact]
        public void Args2()
        {
            var dictionary = Mock.Of<DictionarySvc>();
            Mock.Get(dictionary).Setup(d => d.LookUp(It.IsAny<string>())).Returns("a fish");

            Assert.Equal("a fish", dictionary.LookUp("smelt"));
        }

        [Fact]
        public void ArgsWithPredicate()
        {
            var dictionary = Mock.Of<DictionarySvc>();
            Mock.Get(dictionary).Setup(
                d => d.LookUp(It.Is<string>(s => s.Last() == 's'))).Returns("plural");

            Assert.Equal("plural", dictionary.LookUp("smelts"));
            Assert.Null(dictionary.LookUp("smelt"));
        }

        [Fact]
        public void Args3()
        {
            var mock = new Mock<DictionarySvc>();
            mock.Setup(x => x.StringStuff(It.IsAny<string>()))
                .Returns((string s) => s.ToLower());

            Assert.Equal("bozo", mock.Object.StringStuff("BOZO"));
        }

        [Fact]
        public void Args4()
        {
            // returning different values on each invocation
            var mock = new Mock<DictionarySvc>();
            var i = 0;
            var definitions = new [] { "a fish", "did smell" };
            mock.Setup(d => d.LookUp("smelt"))
                .Returns(() => definitions[i])
                .Callback(() => i++);
            var dict = mock.Object;
            Assert.Equal("a fish", dict.LookUp("smelt"));
            Assert.Equal("did smell", dict.LookUp("smelt"));
        }
    }
}