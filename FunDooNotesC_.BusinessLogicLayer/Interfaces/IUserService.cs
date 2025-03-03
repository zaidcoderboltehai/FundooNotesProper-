using FunDooNotesC_.DataLayer.Entities; // User entity ko use karne ke liye
using System.Threading.Tasks; // Asynchronous operations ko support karne ke liye

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces // Business logic layer ke interfaces ka namespace define kar raha hai
{
    public interface IUserService // IUserService naam ka ek interface bana raha hai
    {
        Task<User> RegisterAsync(User user, string password);
        // Naye user ko register karta hai asynchronously. User object aur password input leta hai aur registered user return karta hai.

        Task<User?> LoginAsync(string email, string password);
        // Existing user ko login karne ke liye, email aur password leta hai.
        // Agar credentials sahi hote hain to User object return karta hai, nahi toh null.

        Task<User?> GetUserByIdAsync(int id);
        // Naya method: User ID ke basis pe registered user ki details fetch karega.
    }
}
