using System;
using System.Linq;
using LibraryNet2020.ControllerHelpers;
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
        public void Add_AssignsCopyNumberWhenNotSpecified()
        {
            var holding = service.Add(new Holding {Classification = "AB123", CopyNumber = 0});

            // var retrievedHolding = context.Holdings.Single(holding => holding.Classification == "AB123");
            Assert.Equal(1, holding.CopyNumber);
        }

        [Fact]
        public void Add_UsesHighwaterCopyNumberWhenAssigning()
        {
            service.Add(new Holding {Classification = "AB123", CopyNumber = 1});
            
            var holding = service.Add(new Holding {Classification = "AB123", CopyNumber = 0});
        
            Assert.Equal("AB123:2", holding.Barcode);
        }

        [Fact]
        public void Add_UsesHighwaterOnlyForBooksWithSameClassification()
        {
            service.Add(new Holding {Classification = "AB123", CopyNumber = 1, HeldByPatronId = 1});
            
            var holding = service.Add(new Holding {Classification = "XX999", CopyNumber = 0, HeldByPatronId = 2});

            Assert.Equal("XX999:1", holding.Barcode);
        }

        // TODO trickle up to controller & have return 400
        [Fact]
        public void Add_ThrowsWhenAddingDuplicateBarcode()
        {
            service.Add(new Holding { Classification = "AB123", CopyNumber = 1 });
        
            var thrown = Assert.Throws<InvalidOperationException>(() => 
                service.Add(new Holding { Classification = "AB123", CopyNumber = 1 }));
            
            Assert.Equal(HoldingsService.ErrorMessageDuplicateBarcode, thrown.Message);
        }
    }
}