using System;
using System.Collections.Generic; // IEnumerable ke liye
using System.Linq.Expressions;      // Expression<Func<T, bool>> ke liye
using System.Threading.Tasks;       // Async programming ke liye

namespace FunDooNotesC_.RepoLayer
{
    // Generic repository interface jo kisi bhi entity ke liye CRUD operations define karta hai.
    // "T" ek generic type hai jo sirf classes ke liye valid hoga.
    public interface IRepository<T> where T : class
    {
        // Sabhi entities ko asynchronously fetch karne ke liye method (without filter).
        Task<IEnumerable<T>> GetAllAsync();

        // Naya method: Predicate ke basis pe filter karke entities ko fetch karega.
        // Yeh Expression<Func<T, bool>> use karta hai taaki query ko SQL me translate kiya ja sake.
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        // Ek specific entity ko uske ID se asynchronously fetch karne ke liye method.
        Task<T?> GetByIdAsync(int id);

        // Nayi entity ko database me asynchronously add karne ke liye method.
        Task AddAsync(T entity);

        // Existing entity ko database me asynchronously update karne ke liye method.
        Task UpdateAsync(T entity);

        // Kisi entity ko ID ke basis pe database se asynchronously delete karne ke liye method.
        Task DeleteAsync(int id);
    }
}
