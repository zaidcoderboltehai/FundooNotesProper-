using FunDooNotesC_.DataLayer; // Data layer ka use ho raha hai
using FunDooNotesC_.RepoLayer; // Repository layer ko include kiya gaya hai
using FunDooNotesC_.BusinessLogicLayer.Interfaces; // Business logic interfaces ko add kiya hai
using FunDooNotesC_.BusinessLogicLayer.Services; // Business logic services ko include kiya hai
using Microsoft.EntityFrameworkCore; // Entity Framework Core ka use ho raha hai (Database ke liye)
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT Authentication ke liye library
using Microsoft.IdentityModel.Tokens; // JWT Token Validation ke liye library
using System.Text; // String encoding ke liye library

var builder = WebApplication.CreateBuilder(args); // Ek web application build karne ka process start ho raha hai

// Services ko container mein add kar rahe hain
builder.Services.AddControllers(); // Controllers ko add kiya jisse API endpoints kaam karein
builder.Services.AddEndpointsApiExplorer(); // API explorer ko enable kiya
builder.Services.AddSwaggerGen(); // Swagger ko enable kiya API documentation ke liye

// JWT configuration setup
var jwtSettings = builder.Configuration.GetSection("Jwt"); // Configuration file se JWT ke settings le rahe hain
var secretKey = jwtSettings["SecretKey"]; // Secret key ko fetch kar rahe hain

// Authentication setup kar rahe hain
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Default authentication JWT set kiya
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Default challenge bhi JWT set kiya
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Issuer ko validate karega (Token kisne issue kiya hai)
        ValidateAudience = true, // Audience ko validate karega (Kaun use kar raha hai)
        ValidateLifetime = true, // Token ka expiry time check karega
        ValidateIssuerSigningKey = true, // Signing key ko validate karega
        ValidIssuer = jwtSettings["Issuer"], // Valid Issuer ko set kiya
        ValidAudience = jwtSettings["Audience"], // Valid Audience ko set kiya
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Secret key ko use karke security key generate ki
    };
});

// Database (EF Core) aur other services ko register kar rahe hain
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Database connection string le rahe hain
        sqlOptions => sqlOptions.MigrationsAssembly("FunDooNotes_App.WebAPI") // Migration assembly specify kiya
    ));

// Dependency Injection setup kiya
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Generic repository ko register kiya
builder.Services.AddScoped<IUserRepository, UserRepository>(); // User repository ko register kiya
builder.Services.AddScoped<IUserService, UserService>(); // User service ko register kiya
builder.Services.AddScoped<INoteService, NoteService>(); // Note service ko register kiya

var app = builder.Build(); // Application build ho raha hai

// Development mode ke liye Swagger enable kiya
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger enable kiya
    app.UseSwaggerUI(); // Swagger UI enable kiya
}

app.UseHttpsRedirection(); // HTTPS redirection enable kiya

// Authentication aur Authorization middleware enable kiya
app.UseAuthentication(); // Authentication middleware add kiya
app.UseAuthorization(); // Authorization middleware add kiya

app.MapControllers(); // Controllers ko map kiya jisse endpoints accessible ho sakein
app.Run(); // Application run kar diya
