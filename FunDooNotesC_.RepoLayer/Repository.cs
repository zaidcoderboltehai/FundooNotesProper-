using Microsoft.EntityFrameworkCore; // Entity Framework Core ka use karne ke liye namespace import kiya hai.
using FunDooNotesC_.DataLayer; // ApplicationDbContext ko access karne ke liye.
using System;
using System.Collections.Generic; // IEnumerable ka use karne ke liye.
using System.Linq; // LINQ operations ko support karne ke liye.
using System.Linq.Expressions; // Expression<Func<T, bool>> ke liye, jo filtering me kaam aata hai.
using System.Threading.Tasks; // Asynchronous programming support karne ke liye.

namespace FunDooNotesC_.RepoLayer
{
    // Generic repository class jo kisi bhi entity (T) ke liye CRUD operations implement karti hai.
    public class Repository<T> : IRepository<T> where T : class
    {
        // ApplicationDbContext ka ek protected field banaya hai, taaki derived classes bhi ise access kar sakein.
        protected readonly ApplicationDbContext _context;

        // Private field jo DbSet<T> ko represent karta hai. Ye database table ke tarah kaam karega.
        private readonly DbSet<T> _dbSet;

        // Constructor jo ApplicationDbContext inject karta hai aur DbSet ko initialize karta hai.
        public Repository(ApplicationDbContext context)
        {
            _context = context; // Injected database context ko local variable me store kar rahe hain.
            _dbSet = _context.Set<T>(); // DbSet<T> ko initialize kar rahe hain, taaki database table se interact kar sakein.
        }

        // Asynchronous method jo sabhi entities ko list ke roop me fetch karega (without filter).
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync(); // DbSet me jitni bhi rows hain unko List me convert karke return karega.
        }

        // Naya asynchronous method jo predicate ke basis pe filtered entities ko fetch karega.
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            // _dbSet.Where(predicate) filtering lagata hai, aur ToListAsync() se result list me convert hota hai.
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // Asynchronous method jo ek specific entity ko uske ID se fetch karega.
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); // ID ke basis par entity dhoondh kar return karega.
        }

        // Asynchronous method jo nayi entity ko database me add karega.
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // Entity ko DbSet me add karega.
            await _context.SaveChangesAsync(); // Changes ko database me save karega.
        }

        // Asynchronous method jo existing entity ko update karega.
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity); // Entity ke update ke liye DbSet ka Update method call karega.
            await _context.SaveChangesAsync(); // Changes ko database me save karega.
        }

        // Asynchronous method jo ID ke basis pe entity ko delete karega.
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id); // Pehle di hui ID se entity ko fetch karega.
            if (entity != null) // Agar entity exist karti hai,
            {
                _dbSet.Remove(entity); // Entity ko DbSet se remove karega.
                await _context.SaveChangesAsync(); // Changes ko database me save karega.
            }
        }
    }
}
