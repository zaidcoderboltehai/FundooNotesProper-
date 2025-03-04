using FunDooNotesC_.BusinessLogicLayer.Helpers; // Helper functions aur classes use karne ke liye include kiya.
using FunDooNotesC_.BusinessLogicLayer.Interfaces; // Interfaces ko use karne ke liye include kiya.
using FunDooNotesC_.DataLayer.Entities; // Data entities (jaise User) access karne ke liye.
using FunDooNotesC_.RepoLayer; // Repository layer se data access karne ke liye.
using System; // Basic system functionalities ke liye.
using System.Threading.Tasks; // Async operations ke liye Task class use karne ke liye.

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces // Is namespace me business logic se related interfaces aur unke implementations rakhe gaye hain.
{
    // UserService class jo IUserService interface ko implement karti hai.
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; // Private field jisme user repository ka instance store hoga.

        // Constructor: Jab UserService ka object create hoga, yahan se repository ka instance milega.
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository; // Injected repository ko assign kar rahe hain.
        }

        // User ko register karega, password hash karega aur database me store karega.
        public async Task<User> RegisterAsync(User user, string password)
        {
            // Pehle check karta hai ki email already exist karta hai ya nahi.
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists"); // Agar email exist karta hai, to error throw karega.

            // Password ko hash karne ke liye helper class ka use karta hai.
            user.PasswordHash = PasswordHelper.HashPassword(user, password);

            await _userRepository.AddAsync(user); // Naya user database me add kar raha hai.
            return user; // Registered user ko return karta hai.
        }

        // User ko login karne ke liye check karta hai ki email aur password sahi hai ya nahi.
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email); // Email ke basis pe user dhoondta hai.
            if (user == null)
                return null; // Agar user nahi mila, to null return karega.

            // Password ko verify karne ke liye helper class ka use karta hai.
            bool isValid = PasswordHelper.VerifyPassword(user, password);
            return isValid ? user : null; // Agar password sahi hai to user return karo, warna null.
        }

        // Naya method: User ko ID ke basis pe fetch karne ke liye.
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id); // ID se user fetch karke return karta hai.
        }
    }
}
