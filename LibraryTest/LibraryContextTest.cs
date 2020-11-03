using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest
{
    [Collection("SharedLibraryContext")]
    public class LibraryContextTest
    {
        private readonly LibraryContext context;

        public LibraryContextTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }
        
        [Fact]
        public void GetByIdReturnsFirstMatch()
        {
            context.Branches.Add(new Branch {Id = 1, Name = "1"});
            context.Branches.Add(new Branch {Id = 2, Name = "2"});
            context.SaveChanges();
            
            var result = context.GetById(context.Branches, 2);
            
            Assert.Equal(2, result.Id);
        }
    }
}