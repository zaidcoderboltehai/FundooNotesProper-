using FunDooNotesC_.ModelLayer;  // Model classes ko access karne ke liye
using FunDooNotesC_.BusinessLogicLayer.Interfaces; // Interfaces ka use karne ke liye
using FunDooNotesC_.DataLayer.Entities; // Database entities ko access karne ke liye
using Microsoft.AspNetCore.Mvc; // API controller banane ke liye
using System; // System-level functionalities ke liye
using System.Threading.Tasks; // Asynchronous operations ke liye

namespace FunDooNotesC_.BusinessLayer.Controllers  // Controller ka namespace define kar raha hai
{
    [ApiController]  // Yeh controller ko API controller ke roop mein identify karta hai
    [Route("api/[controller]")]  // API ka base route set karta hai (e.g., api/auth)
    public class AuthController : ControllerBase  // AuthController bana rahe hain jo ControllerBase inherit karta hai
    {
        private readonly IUserService _userService;  // User ke authentication aur registration ke liye service ka reference

        public AuthController(IUserService userService)  // Constructor jo IUserService ka instance leta hai (Dependency Injection)
        {
            _userService = userService;  // Service ko private field mein assign kar rahe hain
        }

        [HttpPost("register")]  // HTTP POST request ke liye API endpoint define kar raha hai (URL: api/auth/register)
        public async Task<IActionResult> Register([FromBody] RegisterModel request) // Register method jo request body se data lega
        {
            if (!ModelState.IsValid)  // Agar input validation fail ho jaye toh bad request return karega
                return BadRequest(ModelState);

            try
            {
                var user = new User  // Naya User object bana rahe hain
                {
                    FirstName = request.FirstName,  // First name assign kar rahe hain
                    LastName = request.LastName,  // Last name assign kar rahe hain
                    Email = request.Email  // Email assign kar rahe hain
                };

                var registeredUser = await _userService.RegisterAsync(user, request.Password); // User ko register kar rahe hain
                return Ok(new { message = "Registration successful", userId = registeredUser.Id }); // Success response return kar rahe hain
            }
            catch (Exception ex)  // Agar koi error aaye toh exception handle kar rahe hain
            {
                return BadRequest(new { error = ex.Message }); // Error message return kar rahe hain
            }
        }

        [HttpPost("login")]  // HTTP POST request ke liye API endpoint define kar raha hai (URL: api/auth/login)
        public async Task<IActionResult> Login([FromBody] LoginModel request) // Login method jo request body se data lega
        {
            if (!ModelState.IsValid)  // Agar input validation fail ho jaye toh bad request return karega
                return BadRequest(ModelState);

            var user = await _userService.LoginAsync(request.Email, request.Password); // User ko authenticate kar rahe hain
            if (user == null)  // Agar user null return ho toh credentials galat hain
                return Unauthorized(new { error = "Invalid credentials" }); // Unauthorized response return kar rahe hain

            // JWT token generation logic can be added here.  // Yahan JWT token generate karne ka code likh sakte hain
            return Ok(new { message = "Login successful", userId = user.Id }); // Success response return kar rahe hain
        }
    }
}
