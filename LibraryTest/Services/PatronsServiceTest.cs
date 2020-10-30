using LibraryNet2020.Models;
using LibraryNet2020.Services;
using Xunit;

namespace LibraryTest
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
    }
}