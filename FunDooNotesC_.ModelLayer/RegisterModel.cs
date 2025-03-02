using System.ComponentModel.DataAnnotations; // Data validation attributes ke liye namespace import kiya hai

namespace FunDooNotesC_.ModelLayer
{
    // RegisterModel class user registration ka data store karegi
    public class RegisterModel
    {
        [Required] // FirstName ko mandatory banata hai
        [StringLength(50)] // Max 50 characters tak allow karega
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "First name can contain only letters.")] // Sirf alphabets allow karega
        public string FirstName { get; set; } = string.Empty; // Default empty string assign kiya hai

        [Required] // LastName ko mandatory banata hai
        [StringLength(50)] // Max 50 characters tak allow karega
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Last name can contain only letters.")] // Sirf alphabets allow karega
        public string LastName { get; set; } = string.Empty; // Default empty string assign kiya hai

        [Required] // Email ko mandatory banata hai
        [EmailAddress] // Validate karega ki input ek valid email ho
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid email format.")] // Email format aur strict banata hai
        public string Email { get; set; } = string.Empty; // Default empty string assign kiya hai

        [Required] // Password ko mandatory banata hai
        [MinLength(6)] // Minimum 6 characters hona zaroori hai
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Password must be minimum 6 characters, contain at least one uppercase letter, one lowercase letter, one number and one special character.")]
        // Password strong hone chahiye - ek uppercase, ek lowercase, ek number aur ek special character hona zaroori hai
        public string Password { get; set; } = string.Empty; // Default empty string assign kiya hai
    }
}
