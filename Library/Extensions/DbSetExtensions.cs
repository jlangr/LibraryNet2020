using System.Linq;
using System.Threading.Tasks;
using LibraryNet2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryNet2020.Extensions
{
    public static class DbSetExtensions
    {
        public static async Task<T> FirstByIdAsync<T>(this DbSet<T> dbSet, int? id) where T: class, Identifiable
        {
            if (id == null) return default;
            return await dbSet.FirstOrDefaultAsync(m => m.Id == id);
        }

        public static async Task<T> FindByIdAsync<T>(this DbSet<T> dbSet, int? id) where T : class, Identifiable
        {
            if (id == null) return default;
            return await dbSet.FindAsync(id);
        }

        public static bool Exists<T>(this DbSet<T> dbSet, int id) where T : class, Identifiable
        {
            return dbSet.Any(e => e.Id == id);
        }

        public static async void Delete<T>(this DbSet<T> dbSet, int id, DbContext context) where T : class, Identifiable
        {
            var branch = await dbSet.FindAsync(id);
            dbSet.Remove(branch);
            await context.SaveChangesAsync();
        }
    }
}