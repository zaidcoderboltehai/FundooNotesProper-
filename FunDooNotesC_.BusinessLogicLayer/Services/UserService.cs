using FunDooNotesC_.BusinessLogicLayer.Helpers;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.RepoLayer;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning($"Registration attempt with existing email: {user.Email}");
                    throw new Exception("User with this email already exists");
                }

                user.PasswordHash = PasswordHelper.HashPassword(user, password);
                await _userRepository.AddAsync(user);
                _logger.LogInformation($"New user registered: {user.Email}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                throw;
            }
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"Login attempt for non-existent email: {email}");
                    return null;
                }

                bool isValid = PasswordHelper.VerifyPassword(user, password);
                _logger.LogInformation(isValid ?
                    $"Successful login for {email}" :
                    $"Invalid password attempt for {email}");

                return isValid ? user : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"User lookup failed for ID: {id}");
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching user by ID: {id}");
                throw;
            }
        }

        public async Task ForgotPassword(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"Forgot password request for non-existent email: {email}");
                    return;
                }

                user.ResetToken = Guid.NewGuid().ToString();
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                await _userRepository.UpdateAsync(user);

                _logger.LogInformation($"Password reset token generated for {email}");
                // TODO: Add email sending logic here
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in ForgotPassword for {email}");
                throw;
            }
        }

        public async Task<bool> ResetPassword(string email, string token, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null ||
                    user.ResetToken != token ||
                    user.ResetTokenExpiry < DateTime.UtcNow)
                {
                    _logger.LogWarning($"Invalid password reset attempt for {email}");
                    return false;
                }

                user.PasswordHash = PasswordHelper.HashPassword(user, newPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                await _userRepository.UpdateAsync(user);

                _logger.LogInformation($"Password reset successful for {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in ResetPassword for {email}");
                throw;
            }
        }
    }
}