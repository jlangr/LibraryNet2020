using LibraryNet2020.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryTest
{
    public class LibraryControllerTest
    {
        protected LibraryControllerTest(DbContextOptions<LibraryContext> contextOptions)
        {
            ContextOptions = contextOptions;
            Seed();
        }

        protected DbContextOptions<LibraryContext> ContextOptions { get; set; }

        private void Seed()
        {
            using (var context = new LibraryContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // TODO add materials here

                // context.AddRange(objectA, objectB, objectC);

                context.SaveChanges();
            }
        }
    }
}