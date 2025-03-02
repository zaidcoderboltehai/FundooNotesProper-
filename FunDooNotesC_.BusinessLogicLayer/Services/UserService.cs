// Identity management aur repository classes include kar raha hai
using Microsoft.AspNetCore.Identity;                 // Identity library for password hashing
using FunDooNotesC_.BusinessLogicLayer.Interfaces;  // IUserService interface ko implement kar raha hai
using FunDooNotesC_.DataLayer.Entities;             // User entity ka reference le raha hai
using FunDooNotesC_.RepoLayer;                      // UserRepository ka use karega
using System;                                       // Exception handling aur basic functionalities ke liye
using System.Threading.Tasks;                       // Async programming ke liye

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces // Namespace jo services ko define karta hai
{
    // UserService class jo IUserService interface ko implement karti hai
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; // UserRepository ka instance, jo database ke saath interact karega
        private readonly IPasswordHasher<User> _passwordHasher; // Password hashing aur verification ke liye

        // Constructor: Jab UserService ka object create hoga toh repository aur password hasher ka instance milega
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;               // Repository ko assign kar raha hai
            _passwordHasher = new PasswordHasher<User>();   // PasswordHasher ka object bana raha hai
        }

        // User ko register karega, password hash karega aur database me store karega
        public async Task<User> RegisterAsync(User user, string password)
        {
            // Pehle check karega ki email already exist toh nahi karta
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists"); // Agar email pehle se hai toh error throw karega

            // Password ko hash karega (agar "zaid" diya toh uska hashed version store hoga)
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            await _userRepository.AddAsync(user); // Naya user database me add karega
            return user; // Register hua user return karega
        }

        // User ko login karne ke liye check karega ki email aur password sahi hai ya nahi
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email); // Pehle email se user dhoond raha hai
            if (user == null)
                return null; // Agar user nahi mila toh null return karega

            // Password compare karega, hashed password aur entered password ko match karega
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success ? user : null; // Agar password sahi hai toh user return karega, warna null
        }
    }
}
