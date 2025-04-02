using Microsoft.AspNetCore.Mvc;
using FunDooNotesC_.ModelLayer;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.DataLayer.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace FunDooNotesC_.BusinessLayer.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IConfiguration configuration,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        // ------------------------ REGISTER USER ------------------------
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    // New fields
                    Birthday = request.Birthday,
                    Gender = request.Gender
                };

                var registeredUser = await _userService.RegisterAsync(user, request.Password);
                return Ok(new { message = "Registration successful", userId = registeredUser.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error for {Email}", request.Email);
                return BadRequest(new { error = ex.Message });
            }
        }

        // ------------------------ LOGIN USER ------------------------
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userService.LoginAsync(request.Email, request.Password);
                if (user == null)
                    return Unauthorized(new { error = "Invalid credentials" });

                var token = GenerateJwtToken(user);
                return Ok(new { message = "Login successful", token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for {Email}", request.Email);
                return BadRequest(new { error = "Login failed" });
            }
        }

        // ------------------------ GET USER DETAILS ------------------------
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (id != currentUserId)
                    return Forbid();

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                return Ok(new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user {UserId}", id);
                return BadRequest(new { error = "Failed to get user details" });
            }
        }

        // ------------------------ FORGOT PASSWORD ------------------------
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.ForgotPassword(request.Email);
                return Ok(new { message = "If account exists, reset instructions sent" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Forgot password error for {Email}", request.Email);
                return BadRequest(new { error = "Password reset failed. Please try again." });
            }
        }

        // ------------------------ RESET PASSWORD ------------------------
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                bool success = await _userService.ResetPassword(
                    request.Email,
                    request.Token,
                    request.NewPassword
                );

                return success ? Ok(new { message = "Password reset successful" })
                            : BadRequest(new { error = "Invalid or expired reset token" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reset password error for {Email}", request.Email);
                return BadRequest(new { error = "Password reset failed. Please try again." });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["ExpiresInHours"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // ------------------------ Request Models ------------------------
        public class ForgotPasswordRequest
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }
        }

        public class ResetPasswordRequest
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Reset token is required")]
            public string Token { get; set; }

            [Required(ErrorMessage = "New password is required")]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
            public string NewPassword { get; set; }
        }

        // ------------------------ UPDATED RegisterModel ------------------------
        public class RegisterModel
        {
            // Existing properties (e.g. FirstName, LastName, Email, Password)
            [Required(ErrorMessage = "First name is required")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
            public string Password { get; set; }

            // New fields
            [Required(ErrorMessage = "Birthday is required")]
            [DataType(DataType.Date)]
            public DateTime Birthday { get; set; }

            [Required(ErrorMessage = "Gender is required")]
            [StringLength(10)]
            public string Gender { get; set; } // Male/Female/Other
        }
    }
}
