using FunDooNotesC_.BusinessLogicLayer.Helpers;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.RepoLayer;
using System;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLogicLayer.Services // Changed namespace
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new Exception("User with this email already exists");

            user.PasswordHash = PasswordHelper.HashPassword(user, password);
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            bool isValid = PasswordHelper.VerifyPassword(user, password);
            return isValid ? user : null;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}