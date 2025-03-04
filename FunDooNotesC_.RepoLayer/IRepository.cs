using System;
using System.Collections.Generic; // IEnumerable ka use karne ke liye jo ek collection ko represent karta hai
using System.Linq.Expressions;  // Expression<Func<T, bool>> ka use hota hai dynamic queries banane ke liye
using System.Threading.Tasks;   // Async programming aur Task-based asynchronous methods ke liye

namespace FunDooNotesC_.RepoLayer
{
    // Yeh ek generic repository interface hai jo kisi bhi entity ke liye CRUD operations define karta hai.
    // "T" ek generic type hai jo sirf classes ke liye valid hoga.
    public interface IRepository<T> where T : class
    {
        // Sabhi entities ko asynchronously fetch karne ke liye method (without filter).
        Task<IEnumerable<T>> GetAllAsync();

        // Yeh method kisi condition ke basis par entities fetch karega.
        // Expression<Func<T, bool>> ka use hota hai taaki condition SQL query me convert ho sake.
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        // Yeh method ek specific entity ko uski ID ke basis par asynchronously fetch karega.
        // "T?" ka matlab hai ki agar entity na mile toh null return ho sake.
        Task<T?> GetByIdAsync(int id);

        // Yeh method ek naye record ko asynchronously database me add karega.
        Task AddAsync(T entity);

        // Yeh method kisi existing entity ko database me update karega.
        Task UpdateAsync(T entity);

        // Yeh method kisi entity ko ID ke basis pe database se delete karega.
        Task DeleteAsync(int id);
    }
}
