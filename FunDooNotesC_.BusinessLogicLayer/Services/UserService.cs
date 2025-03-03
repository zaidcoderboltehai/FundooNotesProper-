using FunDooNotesC_.BusinessLogicLayer.Helpers; // Include helper namespace
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.RepoLayer;
using System;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces
{
    // UserService class jo IUserService interface ko implement karti hai
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; // UserRepository ka instance

        // Constructor: Jab UserService ka object create hoga, repository ka instance milega
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // User ko register karega, password hash karega aur database me store karega
        public async Task<User> RegisterAsync(User user, string password)
        {
            // Pehle check karega ki email already exist toh nahi karta
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists");

            // Password ko hash karne ke liye helper class ka use karein
            user.PasswordHash = PasswordHelper.HashPassword(user, password);

            await _userRepository.AddAsync(user); // Naya user database me add karega
            return user; // Registered user return karega
        }

        // User ko login karne ke liye check karega ki email aur password sahi hai ya nahi
        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email); // Email se user dhoond raha hai
            if (user == null)
                return null;

            // Password verification helper se
            bool isValid = PasswordHelper.VerifyPassword(user, password);
            return isValid ? user : null;
        }

        // Naya method: User ko ID ke basis par fetch karne ke liye
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}
