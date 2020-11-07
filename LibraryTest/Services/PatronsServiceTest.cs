using LibraryNet2020.Models;
using LibraryNet2020.Services;
using Xunit;

namespace LibraryTest.Services
{
    [Collection("SharedLibraryContext")]
    public class PatronsServiceTest
    {
        private readonly LibraryContext context;
        private readonly PatronsService service;

        public PatronsServiceTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            service = new PatronsService(context);
        }

        [Fact]
        public void HoldingsPopulatesListOfHoldingsForPatron()
        {
            context.Holdings.Add(new Holding {Classification = "AA", HeldByPatronId = 1});
            context.Holdings.Add(new Holding {Classification = "BB", HeldByPatronId = 3});
            context.Holdings.Add(new Holding {Classification = "CC", HeldByPatronId = 5});
            context.Holdings.Add(new Holding {Classification = "DD", HeldByPatronId = 3});
            context.SaveChanges();
                
            var holdings = service.HoldingsForPatron(3);

            Assert.Collection(holdings,
                holding => Assert.Equal("BB", holding.Classification),
                holding => Assert.Equal("DD", holding.Classification));
        }

        [Fact]
        public void CreateSavesRetrievablePatron()
        {
            var idA = service.Create(new Patron {Name = "a"});
            var idB = service.Create(new Patron {Name = "b"});
            
            Assert.Equal(1, idA);
            Assert.Equal(2, idB);

            Assert.Equal("a", service.FindById(1).Name);
            Assert.Equal("b", service.FindById(2).Name);
        }

        class AlwaysFalseCreditVerifier : CreditVerifier
        {
            public bool Verify(string cardNumber) => false;
        }

        [Fact]
        public void CreateFailsIfPatronHasBadCredit()
        {
            var creditVerifier = new AlwaysFalseCreditVerifier();
            service.CreditVerifier = creditVerifier;
            
            var id = service.Create(new Patron {Name = "a"});
            
            Assert.Null(service.FindById(id));
        }
        
        [Fact]
        public void UpdatePersistsChangedPatron()
        {
            var patron = new Patron {Name = "a"};
            var id = service.Create(patron);

            patron.Name = "b";
            service.Update(patron);

            Assert.Equal("b", service.FindById(id).Name);
        }
    }
}