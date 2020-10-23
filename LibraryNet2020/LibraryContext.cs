using Microsoft.EntityFrameworkCore;

namespace LibraryNet2020.Models
{
    
    public class LibraryContext : DbContext
    {
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // See: http://msdn.microsoft.com/en-us/data/jj591621.aspx
        
        public LibraryContext() {}
    
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=Library.db");

        public DbSet<Branch> Branches { get; set; }
    }
}