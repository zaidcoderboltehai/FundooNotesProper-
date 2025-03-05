using System;
using System.Collections.Generic; // IEnumerable ke liye
using System.Linq.Expressions;    // Expression queries ke liye
using System.Threading.Tasks;     // Async operations ke liye

namespace FunDooNotesC_.RepoLayer
{
    // Generic repository interface jo CRUD operations define karta hai
    public interface IRepository<T> where T : class
    {
        // Sabhi entities fetch karo (without filter)
        Task<IEnumerable<T>> GetAllAsync();

        // Condition ke basis par filtered entities fetch karo
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        // ID ke basis par ek entity fetch karo
        Task<T?> GetByIdAsync(int id);

        // Nayi entity add karo
        Task AddAsync(T entity);

        // Existing entity update karo
        Task UpdateAsync(T entity);

        // Entity ko ID ke basis par delete karo (for single-PK tables)
        Task DeleteAsync(int id);

        // Entity directly delete karo (for composite keys)
        Task DeleteAsync(T entity); // ✅ Ye naya method add kiya gaya hai
    }
}