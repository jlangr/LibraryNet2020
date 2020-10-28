using System;
using LibraryNet2020.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryTest
{
    public class DbContextFixture: IDisposable
    {
        public DbContextOptions<LibraryContext> ContextOptions { get; set; }

        public DbContextFixture()
        {
            ContextOptions = new DbContextOptionsBuilder<LibraryContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            Seed();
        }

        public void Seed()
        {
            using var context = new LibraryContext(ContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            // add materials here if necessary
            context.SaveChanges();
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("SharedLibraryContext")]
    public class LibraryContextCollection : ICollectionFixture<DbContextFixture>
    {
        // per XUnit: This class is never created. Its purpose is simply to be the place
        // to apply [CollectionDefinition] and all the ICollectionFixture<> interfaces.
    }
}