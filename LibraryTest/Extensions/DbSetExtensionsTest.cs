using LibraryNet2020.Extensions;
using LibraryNet2020.Models;
using Xunit;

namespace LibraryTest.Extensions
{
    [Collection("SharedLibraryContext")]
    public class DbSetExtensionsTest
    {
        private readonly LibraryContext context;

        public DbSetExtensionsTest(DbContextFixture fixture)
        {
            fixture.Seed();
            context = new LibraryContext(fixture.ContextOptions);
        }

        [Fact]
        public void FindByIdReturnsNullOnNoMatch()
        {
            Assert.Null(context.Branches.FindById(2).Result);
        }

        [Fact]
        public void FindByIdReturnsFirstMatch()
        {
            context.Branches.Add(new Branch {Id = 1, Name = "1"});
            context.Branches.Add(new Branch {Id = 2, Name = "2"});
            context.SaveChanges();
            
            var result = context.Branches.FindById(2).Result;
            
            Assert.Equal(2, result.Id);
        }
    }
}