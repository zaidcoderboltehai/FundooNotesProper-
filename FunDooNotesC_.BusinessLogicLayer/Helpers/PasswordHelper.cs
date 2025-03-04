// File: FunDooNotesC_.BusinessLogicLayer/Helpers/PasswordHelper.cs
// Ye file `PasswordHelper` class ko define karti hai, jo password hashing aur verification ke liye utility methods provide karti hai.

using Microsoft.AspNetCore.Identity;
// Ye line `Microsoft.AspNetCore.Identity` namespace ko include karti hai, jisme password hashing ke liye `IPasswordHasher` interface aur `PasswordHasher` class hai.

using FunDooNotesC_.DataLayer.Entities;
// Ye line `FunDooNotesC_.DataLayer.Entities` namespace ko include karti hai, jisme `User` entity define ki gayi hai.

namespace FunDooNotesC_.BusinessLogicLayer.Helpers
{
    // Ye namespace `FunDooNotesC_.BusinessLogicLayer.Helpers` hai, jisme `PasswordHelper` class rakhi gayi hai.
    // Namespace ka use code ko organize karne ke liye hota hai.

    /// <summary>
    /// Provides utility methods for hashing and verifying user passwords.
    /// </summary>
    // Ye summary comment hai jo batata hai ki `PasswordHelper` class password hashing aur verification ke liye utility methods provide karti hai.

    public static class PasswordHelper
    {
        // `PasswordHelper` ek static class hai, jiska matlab hai ki iski methods ko directly class name se call kiya ja sakta hai (object banane ki zarurat nahi hai).

        // Single instance of IPasswordHasher for consistent hashing
        private static readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        // Ye line ek `IPasswordHasher` ka instance create karti hai, jo password hashing aur verification ke liye use hota hai.
        // `readonly` ka matlab hai ki is variable ko sirf ek baar assign kiya ja sakta hai (consistent hashing ke liye).

        /// <summary>
        /// Hashes the provided plain text password for the specified user.
        /// </summary>
        /// <param name="user">User entity for which the password is being hashed.</param>
        /// <param name="password">Plain text password.</param>
        /// <returns>Hashed password string.</returns>
        // Ye summary comment `HashPassword` method ke bare mein batata hai. Is method ka use plain text password ko hash karne ke liye hota hai.

        public static string HashPassword(User user, string password)
        {
            // Ye method ek plain text password ko hash karke uska hashed version return karta hai.
            return _passwordHasher.HashPassword(user, password);
            // `_passwordHasher.HashPassword` method plain text password ko hash karta hai aur hashed password return karta hai.
        }

        /// <summary>
        /// Verifies if the provided plain text password matches the hashed password stored for the user.
        /// </summary>
        /// <param name="user">User entity whose password is being verified.</param>
        /// <param name="providedPassword">Plain text password to verify.</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        // Ye summary comment `VerifyPassword` method ke bare mein batata hai. Is method ka use plain text password ko verify karne ke liye hota hai.

        public static bool VerifyPassword(User user, string providedPassword)
        {
            // Ye method check karta hai ki provided plain text password user ke stored hashed password se match karta hai ya nahi.
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
            // `_passwordHasher.VerifyHashedPassword` method hashed password aur plain text password ko compare karta hai.

            return verificationResult == PasswordVerificationResult.Success;
            // Agar password match karta hai, toh `PasswordVerificationResult.Success` return hota hai, aur method `true` return karta hai.
            // Agar password match nahi karta, toh `false` return hota hai.
        }
    }
}