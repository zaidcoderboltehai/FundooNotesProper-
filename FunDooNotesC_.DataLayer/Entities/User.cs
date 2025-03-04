using System.Collections.Generic; // Collections, jaise List, use karne ke liye.
using System.ComponentModel.DataAnnotations; // Data validation attributes (e.g., [Required], [StringLength]) use karne ke liye.

namespace FunDooNotesC_.DataLayer.Entities // Namespace jisme data entities rakhe jaate hain.
{
    public class User // User entity class, jo user ka data represent karta hai.
    {
        public int Id { get; set; } // User ka unique identifier (ID).

        [Required(ErrorMessage = "First name is required")] // First name zaroori hai; agar missing ho to error message de.
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")] // First name 50 characters se zyada allowed nahi.
        public string FirstName { get; set; } = string.Empty; // FirstName property, default empty string se initialize.

        [Required(ErrorMessage = "Last name is required")] // Last name required hai; agar missing ho to error message de.
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")] // Last name ki length 50 se zyada allowed nahi.
        public string LastName { get; set; } = string.Empty; // LastName property, default empty string.

        [Required(ErrorMessage = "Email is required")] // Email field required hai.
        [EmailAddress(ErrorMessage = "Invalid email format (e.g., user@example.com)")] // Email ko validate karta hai sahi format ke liye.
        public string Email { get; set; } = string.Empty; // Email property, default empty string se initialize.

        [Required(ErrorMessage = "Password hash is required")] // Password hash zaroori hai; agar nahi diya to error message de.
        public string PasswordHash { get; set; } = string.Empty; // PasswordHash property, hashed password store karta hai.

        // Navigation Property: A user can have multiple notes
        public ICollection<Note> Notes { get; set; } = new List<Note>(); // Notes collection, jo user ke sabhi notes rakhta hai.
    }
}
