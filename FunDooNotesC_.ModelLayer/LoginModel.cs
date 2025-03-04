using System.ComponentModel.DataAnnotations; // Data validation attributes ke liye namespace import kiya hai

namespace FunDooNotesC_.ModelLayer // Namespace define kar raha hai jo is class ko organize karega
{
    // LoginModel class user ka login data store karegi
    public class LoginModel
    {
        [Required] // Yeh ensure karega ki Email field blank na ho, yani input dena zaroori hai
        [EmailAddress] // Yeh check karega ki Email ek valid email address format me ho
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid email format.")] // Yeh aur strict validation lagayega ki email sahi format me ho
        public string Email { get; set; } = string.Empty; // Default empty string assign kiya hai taaki null na ho

        [Required] // Yeh ensure karega ki Password field blank na ho, yani input dena zaroori hai
        public string Password { get; set; } = string.Empty; // Default empty string assign kiya hai taaki null na ho
    }
}
