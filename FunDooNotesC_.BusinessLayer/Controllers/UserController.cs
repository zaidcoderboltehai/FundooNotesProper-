using Microsoft.AspNetCore.Mvc;
using FunDooNotesC_.ModelLayer;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

// Ye sab 'using' statements hain jo required namespaces ko include karte hain.
// Jaise:
// - Microsoft.AspNetCore.Mvc: API controllers aur actions banane ke liye.
// - FunDooNotesC_.ModelLayer: Models (jaise RegisterModel, LoginModel) ke liye.
// - FunDooNotesC_.BusinessLogicLayer.Interfaces: Business logic interfaces ke liye.
// - FunDooNotesC_.DataLayer.Entities: Database entities (jaise User) ke liye.
// - Microsoft.IdentityModel.Tokens: JWT tokens ke liye.
// - System.IdentityModel.Tokens.Jwt: JWT tokens create aur validate karne ke liye.
// - System.Security.Claims: User claims (jaise user ID) access karne ke liye.
// - System.Text: String encoding ke liye.
// - Microsoft.AspNetCore.Authorization: Authorization (user permissions) ke liye.

namespace FunDooNotesC_.BusinessLayer.Controllers
{
    // Ye namespace define karta hai ki ye controller kahan exist karta hai.

    [ApiController]
    // Ye attribute batata hai ki ye class ek API controller hai.
    // API controllers HTTP requests ko handle karte hain.

    [Route("api/User")]
    // Ye attribute API ka route define karta hai.
    // Iska matlab ye API endpoint `/api/User` pe available hoga.

    public class UserController : ControllerBase
    {
        // Ye class UserController hai jo ControllerBase se inherit karti hai.
        // ControllerBase ek base class hai jo API controllers ke liye common functionality provide karti hai.

        private readonly IUserService _userService;
        // Ye ek private field hai jo IUserService interface ko store karta hai.
        // Iska use user-related business logic ko handle karne ke liye hota hai.

        private readonly IConfiguration _configuration;
        // Ye ek private field hai jo appsettings.json se configuration data (jaise JWT settings) access karne ke liye use hota hai.

        public UserController(IUserService userService, IConfiguration configuration)
        {
            // Ye constructor hai jo dependency injection ke through IUserService aur IConfiguration ke instances leta hai.
            // Dependency injection ka matlab hai ki framework automatically services provide karega.
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        // Ye HTTP POST request ko handle karta hai, jo user registration ke liye hai.
        // Iska matlab hai ki jab `/api/User/register` pe POST request aayegi, ye method chalega.

        [AllowAnonymous]
        // Ye attribute batata hai ki is action ko access karne ke liye user ko logged in hona zaroori nahi hai.
        // Kyunki registration ke time user logged in nahi hoga.

        public async Task<IActionResult> Register([FromBody] RegisterModel request)
        {
            // Ye method user registration ko handle karta hai.

            if (!ModelState.IsValid)
                // Agar request data valid nahi hai (jaise first name missing hai), toh 400 Bad Request return kiya jata hai.
                return BadRequest(ModelState);

            try
            {
                var user = new User
                {
                    // RegisterModel se data lekar ek new User object banaya jata hai.
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email
                };

                var registeredUser = await _userService.RegisterAsync(user, request.Password);
                // User ko database mein register kiya jata hai.

                return Ok(new { message = "Registration successful", userId = registeredUser.Id });
                // HTTP 200 OK status ke saath success message aur user ID return kiya jata hai.
            }
            catch (Exception ex)
            {
                // Agar koi error aata hai (jaise email already exists), toh 400 Bad Request return kiya jata hai.
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        // Ye HTTP POST request ko handle karta hai, jo user login ke liye hai.
        // Iska matlab hai ki jab `/api/User/login` pe POST request aayegi, ye method chalega.

        [AllowAnonymous]
        // Ye attribute batata hai ki is action ko access karne ke liye user ko logged in hona zaroori nahi hai.
        // Kyunki login ke time user logged in nahi hoga.

        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            // Ye method user login ko handle karta hai.

            if (!ModelState.IsValid)
                // Agar request data valid nahi hai (jaise email missing hai), toh 400 Bad Request return kiya jata hai.
                return BadRequest(ModelState);

            var user = await _userService.LoginAsync(request.Email, request.Password);
            // User ko email aur password ke basis pe verify kiya jata hai.

            if (user == null)
                // Agar user nahi mila ya password galat hai, toh 401 Unauthorized return kiya jata hai.
                return Unauthorized(new { error = "Invalid credentials" });

            var token = GenerateJwtToken(user);
            // JWT token generate kiya jata hai.

            return Ok(new { message = "Login successful", token });
            // HTTP 200 OK status ke saath success message aur JWT token return kiya jata hai.
        }

        [HttpGet("{id}")]
        // Ye HTTP GET request ko handle karta hai, jo specific user ke details fetch karne ke liye hai.
        // {id} ek route parameter hai, jaise `/api/User/1`.

        [Authorize]
        // Ye attribute batata hai ki is action ko access karne ke liye user ko logged in hona zaroori hai.

        public async Task<IActionResult> GetUserDetails(int id)
        {
            // Ye method user details ko fetch karta hai.

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // Current logged-in user ka ID fetch kiya jata hai.

            if (id != currentUserId)
                // Agar requested user ID aur current user ID match nahi karte, toh 403 Forbidden return kiya jata hai.
                return Forbid();

            var user = await _userService.GetUserByIdAsync(id);
            // User ko ID ke basis pe fetch kiya jata hai.

            if (user == null)
                // Agar user nahi mila, toh 404 Not Found return kiya jata hai.
                return NotFound();

            return Ok(new
            {
                // User details ko HTTP 200 OK status ke saath return kiya jata hai.
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email
            });
        }

        private string GenerateJwtToken(User user)
        {
            // Ye method JWT token generate karta hai.

            var jwtSettings = _configuration.GetSection("Jwt");
            // appsettings.json se JWT settings fetch kiye jaate hain.

            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
            // Secret key ko bytes mein convert kiya jata hai.

            var claims = new List<Claim>
            {
                // User ke claims (information) define kiye jaate hain.
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID
                new Claim(ClaimTypes.Email, user.Email), // User email
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}") // User full name
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Token ke properties define kiye jaate hain.
                Subject = new ClaimsIdentity(claims), // User claims
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpiresInHours"])), // Token expiry time
                Issuer = jwtSettings["Issuer"], // Token issuer
                Audience = jwtSettings["Audience"], // Token audience
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), // Signing key
                    SecurityAlgorithms.HmacSha256Signature) // Signing algorithm
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            // JWT token handler create kiya jata hai.

            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Token generate kiya jata hai.

            return tokenHandler.WriteToken(token);
            // Token ko string format mein return kiya jata hai.
        }
    }
}