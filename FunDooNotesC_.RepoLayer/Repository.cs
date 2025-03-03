using Microsoft.EntityFrameworkCore; // EF Core ke liye
using FunDooNotesC_.DataLayer;         // ApplicationDbContext ke liye
using System;
using System.Collections.Generic;     // IEnumerable ke liye
using System.Linq;                     // LINQ ke liye
using System.Linq.Expressions;         // Expression<Func<T, bool>> ke liye
using System.Threading.Tasks;          // Async methods ke liye

namespace FunDooNotesC_.RepoLayer
{
    // Generic repository class jo kisi bhi entity (T) ke liye CRUD operations implement karti hai.
    public class Repository<T> : IRepository<T> where T : class
    {
        // Protected field jo ApplicationDbContext ko store karta hai. Derived classes bhi ise access kar sakti hain.
        protected readonly ApplicationDbContext _context;

        // Private field jo DbSet<T> ko represent karta hai. Ye specific entity ki table ke jaisa hota hai.
        private readonly DbSet<T> _dbSet;

        // Constructor jo ApplicationDbContext inject karta hai aur DbSet ko initialize karta hai.
        public Repository(ApplicationDbContext context)
        {
            _context = context; // Injected database context assign kar rahe hain.
            _dbSet = _context.Set<T>(); // DbSet ko initialize kar rahe hain.
        }

        // Asynchronous method jo sabhi entities ko list ke roop me fetch karta hai (no filter).
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync(); // DbSet se saari rows ko list me convert karke return karta hai.
        }

        // Naya asynchronous method jo predicate ke basis pe filtered entities ko fetch karta hai.
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            // _dbSet.Where(predicate) se filter apply hota hai, aur ToListAsync() se result list me convert hota hai.
            return await _dbSet.Where(predicate).ToListAsync();
        }

        // Asynchronous method jo ek specific entity ko uske ID se fetch karta hai.
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); // ID ke basis par entity find karta hai.
        }

        // Asynchronous method jo nayi entity ko database me add karta hai.
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // Entity ko DbSet me add karta hai.
            await _context.SaveChangesAsync(); // Changes ko database me save karta hai.
        }

        // Asynchronous method jo existing entity ko update karta hai.
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity); // Entity ke update ke liye DbSet ka Update method call karta hai.
            await _context.SaveChangesAsync(); // Changes ko database me save karta hai.
        }

        // Asynchronous method jo ID ke basis pe entity ko delete karta hai.
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id); // Di hui ID se entity fetch karta hai.
            if (entity != null) // Agar entity exist karti hai,
            {
                _dbSet.Remove(entity); // Entity ko DbSet se remove karta hai.
                await _context.SaveChangesAsync(); // Changes ko database me save karta hai.
            }
        }
    }
}
