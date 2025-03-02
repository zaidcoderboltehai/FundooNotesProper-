// FunDooNotes_App.DAL/Entities/User.cs

using System.ComponentModel.DataAnnotations; // Data validation attributes use karne ke liye

namespace FunDooNotesC_.DataLayer.Entities
{
    // User class jo ek user ka data store karega
    public class User
    {
        // Har ek user ka unique ID hoga (Primary Key in database)
        public int Id { get; set; }

        [Required] // First name ka value required hai (empty nahi reh sakta)
        [StringLength(50)] // Max 50 characters tak allowed hai
        public string FirstName { get; set; } = string.Empty;

        [Required] // Last name ka value bhi required hai
        [StringLength(50)] // Max 50 characters allowed hai
        public string LastName { get; set; } = string.Empty;

        [Required] // Email bhi required hai
        [EmailAddress] // Valid email format hona chahiye
        public string Email { get; set; } = string.Empty;

        // User ka password securely store karne ke liye hashed format me rakha jayega
        public string PasswordHash { get; set; } = string.Empty;

        // Agar kisi user ke multiple notes hote hain, toh usko store karne ke liye
        // public ICollection<Note> Notes { get; set; } // Relationship establish karega (Optional)
    }
}
