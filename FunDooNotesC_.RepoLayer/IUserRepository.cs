using FunDooNotesC_.DataLayer.Entities; // User entity ko use karne ke liye namespace include kiya gaya hai
using System.Threading.Tasks; // Asynchronous programming support karne ke liye

namespace FunDooNotesC_.RepoLayer
{
    // IUserRepository interface, jo IRepository<User> ko inherit kar raha hai
    public interface IUserRepository : IRepository<User>
    {
        // Specific user ko email ke basis pe fetch karne ke liye asynchronous method
        Task<User?> GetByEmailAsync(string email);
    }
}
