// Inbuilt namespaces import kar rahe hain jo zaruri classes provide karte hain
using FunDooNotesC_.ModelLayer;                                // Models ke liye (jaise RegisterModel, LoginModel)
using FunDooNotesC_.BusinessLogicLayer.Interfaces;           // Business logic interfaces (IUserService)
using FunDooNotesC_.DataLayer.Entities;                      // Data layer ke entities (User)
using Microsoft.AspNetCore.Mvc;                              // ASP.NET Core MVC functionalities ke liye
using Microsoft.IdentityModel.Tokens;                        // Token related functionalities ke liye (SigningCredentials, etc.)
using System.IdentityModel.Tokens.Jwt;                       // JWT token generation aur handling ke liye
using System.Security.Claims;                                // Claims banane ke liye (user ki details)
using System.Text;                                           // Encoding functionalities ke liye (UTF8 conversion)

namespace FunDooNotesC_.BusinessLogicLayer.Controllers
{
    // [ApiController] attribute batata hai ki yeh controller API responses dega
    [ApiController]
    // [Route("api/[controller]")] se base route set ho jata hai, yahan "api/Auth" hoga
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Private field jismein IUserService ka instance store hoga (business logic ke liye)
        private readonly IUserService _userService;
        // Private field jismein configuration settings (appsettings.json se) milegi
        private readonly IConfiguration _configuration;

        // Constructor: Dependency Injection se IUserService aur IConfiguration milte hain
        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;          // Injected user service ko assign kar rahe hain
            _configuration = configuration;      // Injected configuration ko assign kar rahe hain
        }

        // HTTP POST request ke liye "register" endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel request)
        {
            // Agar model validation fail ho jaye, to BadRequest return karo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Naya user object create kar rahe hain aur properties assign kar rahe hain
                var user = new User
                {
                    FirstName = request.FirstName,   // Request se first name le rahe hain
                    LastName = request.LastName,     // Request se last name le rahe hain
                    Email = request.Email            // Request se email le rahe hain
                };

                // User ko register karne ke liye service call kar rahe hain (password hashing bhi yahin hoti hai)
                var registeredUser = await _userService.RegisterAsync(user, request.Password);
                // Agar successful, to success message ke sath registered user's Id return karo
                return Ok(new { message = "Registration successful", userId = registeredUser.Id });
            }
            catch (Exception ex)
            {
                // Agar koi error aata hai, to error message ke sath BadRequest return karo
                return BadRequest(new { error = ex.Message });
            }
        }

        // HTTP POST request ke liye "login" endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            // Model validation check kar rahe hain, agar fail ho to BadRequest
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // User ko login ke liye service se authenticate kar rahe hain
            var user = await _userService.LoginAsync(request.Email, request.Password);
            // Agar user null hai, matlab credentials galat hain, Unauthorized return karo
            if (user == null)
                return Unauthorized(new { error = "Invalid credentials" });

            // Yahan se JWT token generate karne ka process shuru hota hai

            // Configuration se "Jwt" section le rahe hain
            var jwtSettings = _configuration.GetSection("Jwt");
            // Secret key, issuer, audience aur expiry time retrieve kar rahe hain
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiresInHours = Convert.ToDouble(jwtSettings["ExpiresInHours"]);

            // JWT token generate karne ke liye handler banate hain
            var tokenHandler = new JwtSecurityTokenHandler();
            // Secret key ko bytes mein convert kar rahe hain using UTF8 encoding
            var key = Encoding.UTF8.GetBytes(secretKey);

            // Claims set kar rahe hain jismein user ki details hongi (ye token ke andar stored hongi)
            var claims = new List<Claim>
            {
                // User ka unique ID claim ke roop mein
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // User ka email claim ke roop mein
                new Claim(ClaimTypes.Email, user.Email),
                // User ka full name claim ke roop mein
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
            };

            // Token descriptor banate hain jo token ke properties define karta hai
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Token ke andar ye claims honge
                Subject = new ClaimsIdentity(claims),
                // Token ki expiry time set kar rahe hain (abhi se calculated expiry)
                Expires = DateTime.UtcNow.AddHours(expiresInHours),
                // Issuer aur Audience set kar rahe hain (ye token validation ke liye use honge)
                Issuer = issuer,
                Audience = audience,
                // Token ko sign karne ke liye credentials set kar rahe hain using HMAC-SHA256 algorithm
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Token create kar rahe hain token handler se
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Token ko string format mein likh rahe hain (jo client ko bheja jayega)
            var tokenString = tokenHandler.WriteToken(token);

            // Successful login ke response mein message aur JWT token return kar rahe hain
            return Ok(new { message = "Login successful", token = tokenString });
        }
    }
}
