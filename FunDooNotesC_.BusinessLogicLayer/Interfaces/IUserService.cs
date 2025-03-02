// Yeh file FunDooNotes_App ki Business Logic Layer me Interfaces ka part hai
// Yahaan par user authentication ke liye service interface define kiya gaya hai

using FunDooNotesC_.DataLayer.Entities; // User entity ko use karne ke liye
using System.Threading.Tasks; // Asynchronous operations ko support karne ke liye

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces // Business logic layer ke interfaces ka namespace define kar raha hai
{
    public interface IUserService // IUserService naam ka ek interface bana raha hai
    {
        Task<User> RegisterAsync(User user, string password);
        // Yeh method naye user ko register karega asynchronously
        // User object aur password ko input lega
        // Ek user object return karega jab registration complete ho jayega

        Task<User?> LoginAsync(string email, string password);
        // Yeh method existing user ko login karne ke liye hai
        // Email aur password ko input lega
        // Agar credentials sahi honge toh User return karega, nahi toh null
    }
}
