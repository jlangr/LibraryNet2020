using System.Collections.Generic;
using System.Linq;
using LibraryNet2020.ControllerHelpers;
using LibraryNet2020.NonPersistentModels;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holding>()
                .Ignore(holding => holding.CheckoutPolicy);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("DataSource=Library.db");

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<Material> Materials { get; set; }

        public T GetById<T>(DbSet<T> dbSet, int id) where T: class, Identifiable
        {
            return dbSet.FirstOrDefault(e => e.Id == id);
        }
    }
    
}