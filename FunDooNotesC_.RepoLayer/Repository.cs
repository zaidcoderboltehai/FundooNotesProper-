using Microsoft.EntityFrameworkCore; // EF Core ka use karne ke liye, jisse database se interact karte hain
using FunDooNotesC_.DataLayer; // DataLayer project se ApplicationDbContext ko access karne ke liye
using System.Collections.Generic; // Collections jaise List aur IEnumerable ke liye
using System.Threading.Tasks; // Async methods ke liye Task ka use karne ke liye

namespace FunDooNotesC_.RepoLayer // RepoLayer project ka namespace
{
    // Generic repository class jo kisi bhi entity (T) ke liye CRUD (Create, Read, Update, Delete) operations implement karti hai.
    public class Repository<T> : IRepository<T> where T : class
    {
        // Protected field jo ApplicationDbContext ko store karta hai. Isko derived classes bhi access kar sakti hain.
        protected readonly ApplicationDbContext _context;

        // Private field jo DbSet of T ko represent karta hai. Ye specific entity ki table ke jaisa hota hai.
        private readonly DbSet<T> _dbSet;

        // Constructor jo ApplicationDbContext inject karta hai aur DbSet ko initialize karta hai.
        public Repository(ApplicationDbContext context)
        {
            _context = context; // Injected database context ko assign karta hai.
            _dbSet = _context.Set<T>(); // DbSet ko initialize karta hai jisse hum CRUD operations perform karte hain.
        }

        // Asynchronous method jo saari entities ko list ke roop me fetch karta hai.
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync(); // DbSet se saari rows ko list me convert karke return karta hai.
        }

        // Asynchronous method jo ek specific entity ko uske ID se fetch karta hai.
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id); // ID ke basis par entity find karta hai. Agar entity na mile toh null return karega.
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

        // Asynchronous method jo ID ke basis par entity ko delete karta hai.
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id); // Pehle, di hui ID se entity ko fetch karta hai.
            if (entity != null) // Agar entity exist karti hai,
            {
                _dbSet.Remove(entity); // Entity ko DbSet se remove karta hai.
                await _context.SaveChangesAsync(); // Changes ko database me save karta hai.
            }
        }
    }
}
