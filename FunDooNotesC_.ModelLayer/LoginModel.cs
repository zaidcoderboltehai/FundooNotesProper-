using System.ComponentModel.DataAnnotations; // Data validation attributes ke liye namespace import kiya hai

namespace FunDooNotesC_.ModelLayer
{
    // LoginModel class user ka login data store karegi
    public class LoginModel
    {
        [Required] // Email field ko mandatory banata hai
        [EmailAddress] // Validate karega ki input ek valid email address ho
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid email format.")] // Email ke format ko aur strict banata hai
        public string Email { get; set; } = string.Empty; // Default empty string assign ki hai

        [Required] // Password field ko mandatory banata hai
        public string Password { get; set; } = string.Empty; // Default empty string assign ki hai
    }
}
