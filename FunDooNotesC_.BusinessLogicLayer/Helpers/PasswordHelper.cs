// File: FunDooNotesC_.BusinessLogicLayer/Helpers/PasswordHelper.cs
using Microsoft.AspNetCore.Identity;
using FunDooNotesC_.DataLayer.Entities;

namespace FunDooNotesC_.BusinessLogicLayer.Helpers
{
    /// <summary>
    /// Provides utility methods for hashing and verifying user passwords.
    /// </summary>
    public static class PasswordHelper
    {
        // Single instance of IPasswordHasher for consistent hashing
        private static readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        /// <summary>
        /// Hashes the provided plain text password for the specified user.
        /// </summary>
        /// <param name="user">User entity for which the password is being hashed.</param>
        /// <param name="password">Plain text password.</param>
        /// <returns>Hashed password string.</returns>
        public static string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        /// <summary>
        /// Verifies if the provided plain text password matches the hashed password stored for the user.
        /// </summary>
        /// <param name="user">User entity whose password is being verified.</param>
        /// <param name="providedPassword">Plain text password to verify.</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        public static bool VerifyPassword(User user, string providedPassword)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}
