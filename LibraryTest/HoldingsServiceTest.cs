using System.Linq;
using LibraryNet2020.Controllers;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest
{
    [Collection("SharedLibraryContext")]
    public class HoldingsServiceTest
    {
        private readonly LibraryContext context;
        private readonly HoldingsService service;

        public HoldingsServiceTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
            service = new HoldingsService(context);
        }
        
        [Fact]
        public void Create_AssignsCopyNumber()
        {
            service.Add(new Holding { Classification = "AB123", CopyNumber = 0 });

            var retrievedHolding = context.Holdings.Single(holding => holding.Classification == "AB123");
            Assert.Equal(1, retrievedHolding.CopyNumber);
        }
    }
}