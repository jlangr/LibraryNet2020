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
            Assert.Null(context.Branches.FirstByIdAsync(2).Result);
        }

        [Fact]
        public void FindByIdReturnsNullOnNullId()
        {
            Assert.Null(context.Branches.FirstByIdAsync(null).Result);
        }

        [Fact]
        public void FirstByIdAsyncReturnsFirstMatch()
        {
            context.Branches.Add(new Branch {Id = 1, Name = "1"});
            context.Branches.Add(new Branch {Id = 2, Name = "2"});
            context.SaveChanges();

            var result = context.Branches.FirstByIdAsync(2).Result;

            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void FirstByIdAsyncReturnsNullOnNoMatch()
        {
            Assert.Null(context.Branches.FindByIdAsync(2).Result);
        }

        [Fact]
        public void FindByIdAsyncReturnsNullOnNullId()
        {
            Assert.Null(context.Branches.FindByIdAsync(null).Result);
        }

        [Fact]
        public void FindByIdAsyncReturnsFirstMatch()
        {
            context.Branches.Add(new Branch {Id = 1, Name = "1"});
            context.Branches.Add(new Branch {Id = 2, Name = "2"});
            context.SaveChanges();

            var result = context.Branches.FindByIdAsync(2).Result;

            Assert.Equal(2, result.Id);
        }

        [Collection("SharedLibraryContext")]
        public class ExistsTest
        {
            private readonly LibraryContext context;

            public ExistsTest(DbContextFixture fixture)
            {
                fixture.Seed();
                context = new LibraryContext(fixture.ContextOptions);
            }
            
            [Fact]
            public void ExistsReturnsFalseOnNoIdMatch()
            {
                context.Branches.Add(new Branch {Id = 2, Name = "2"});
                context.SaveChanges();

                var result = context.Branches.Exists(3);

                Assert.False(result);
            }

            [Fact]
            public void ExistsReturnsTrueOnIdMatch()
            {
                context.Branches.Add(new Branch {Id = 1, Name = "1"});
                context.Branches.Add(new Branch {Id = 2, Name = "2"});
                context.SaveChanges();

                var result = context.Branches.Exists(2);

                Assert.True(result);
            }
        }
    }
}