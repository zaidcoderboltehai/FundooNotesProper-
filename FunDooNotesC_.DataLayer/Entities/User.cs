using System;
using System.Collections.Generic;            // For collections like List.
using System.ComponentModel.DataAnnotations; // For validation attributes ([Required], [StringLength], etc.).

namespace FunDooNotesC_.DataLayer.Entities   // Namespace for data entities
{
    public class User
    {
        public int Id { get; set; }          // Unique ID
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format (e.g., user@example.com)")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password hash is required")]
        public string PasswordHash { get; set; } = string.Empty;

        // Navigation property: A user can have multiple notes
        public ICollection<Note> Notes { get; set; } = new List<Note>();

        // --- New Properties ---
        [Required(ErrorMessage = "Birthday is required")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty; // Male/Female/Other
    }
}
