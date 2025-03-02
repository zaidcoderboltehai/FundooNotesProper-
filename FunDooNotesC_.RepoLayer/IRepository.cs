// FunDooNotes_App.DAL/Repositories/IRepository.cs

using System.Collections.Generic; // IEnumerable use karne ke liye
using System.Threading.Tasks; // Async programming support karne ke liye

namespace FunDooNotesC_.RepoLayer
{
    // Generic repository interface jo kisi bhi entity ke liye CRUD (Create, Read, Update, Delete) operations define karta hai
    public interface IRepository<T> where T : class // T ek generic type hai jo sirf classes ke liye valid hoga
    {
        // Saari entities ko asynchronously fetch karne ke liye method
        Task<IEnumerable<T>> GetAllAsync();

        // Ek specific entity ko uske ID se asynchronously fetch karne ke liye method
        Task<T?> GetByIdAsync(int id);

        // Nayi entity ko database me asynchronously add karne ke liye method
        Task AddAsync(T entity);

        // Existing entity ko database me asynchronously update karne ke liye method
        Task UpdateAsync(T entity);

        // Kisi entity ko ID ke basis pe database se asynchronously delete karne ke liye method
        Task DeleteAsync(int id);
    }
}
