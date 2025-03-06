using FunDooNotesC_.DataLayer.Entities; // User entity ko use karne ke liye
// Ye line `FunDooNotesC_.DataLayer.Entities` namespace ko include karti hai, jisme `User` entity define ki gayi hai.
// `User` entity user-related data ko represent karti hai.

using System.Threading.Tasks; // Asynchronous operations ko support karne ke liye
// Ye line `System.Threading.Tasks` namespace ko include karti hai, jisme `Task` aur async programming ke liye classes hote hain.
// Async programming ka use hota hai taaki hum blocking (ruke bina) operations perform kar sakein.

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces // Business logic layer ke interfaces ka namespace define kar raha hai
{
    // Ye namespace `FunDooNotesC_.BusinessLogicLayer.Interfaces` hai, jisme `IUserService` interface rakha gaya hai.
    // Namespace ka use code ko organize karne ke liye hota hai.

    public interface IUserService // IUserService naam ka ek interface bana raha hai
    {
        // `IUserService` ek interface hai jo user-related operations ko define karta hai.
        // Interface ek contract ki tarah hota hai, jisme methods ke signatures define hote hain, but unka implementation nahi hota.

        Task<User> RegisterAsync(User user, string password);
        // Ye method ek naye user ko register karta hai asynchronously.
        // `User` object aur `password` input mein leta hai aur registered `User` object return karta hai.
        // `Task<User>` ka matlab hai ki yeh method async hai aur ek `User` object return karega.

        Task<User?> LoginAsync(string email, string password);
        // Ye method existing user ko login karne ke liye use hota hai.
        // `email` aur `password` input mein leta hai aur agar credentials sahi hote hain, toh `User` object return karta hai.
        // Agar credentials sahi nahi hote, toh `null` return karta hai.
        // `Task<User?>` ka matlab hai ki yeh method async hai aur ek `User` object ya `null` return karega.

        Task<User?> GetUserByIdAsync(int id);
        // Ye method user ki details ko uske `id` ke basis par fetch karta hai.
        // `id` input mein leta hai aur agar user milta hai, toh `User` object return karta hai, nahi toh `null`.
        // `Task<User?>` ka matlab hai ki yeh method async hai aur ek `User` object ya `null` return karega.

        Task ForgotPassword(string email);
        Task<bool> ResetPassword(string email, string token, string newPassword);
    }
}