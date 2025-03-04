using FunDooNotesC_.DataLayer;
using FunDooNotesC_.RepoLayer;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.BusinessLogicLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
// Ye sab 'using' statements hain. Inka matlab hai ki hum in libraries ya namespaces ka code use kar rahe hain.
// Example: `Microsoft.EntityFrameworkCore` database se related kaam ke liye hai.

var builder = WebApplication.CreateBuilder(args);
// Ye line ek web application ka builder create karta hai. Isse hum apne app ki settings aur services configure karte hain.

// Services Configuration
builder.Services.AddControllers();
// Ye line batati hai ki hum apne app mein controllers use karenge. Controllers API endpoints handle karte hain.

builder.Services.AddEndpointsApiExplorer();
// Ye line API endpoints ko explore karne ke liye swagger ko enable karta hai.

// Swagger Configuration with JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Token format: 'Bearer <your-token>'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    // Ye Swagger ko batata hai ki hum JWT token use kar rahe hain aur token ka format kya hai.

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    // Ye Swagger ko batata hai ki har API call ke liye JWT token required hai.
});

// JWT Authentication Setup
var jwtSettings = builder.Configuration.GetSection("Jwt");
// Ye line appsettings.json se JWT settings ko fetch karta hai.

var secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentNullException("JWT Secret Key is missing");
// Ye JWT secret key ko fetch karta hai. Agar key missing hai, toh error throw karta hai.

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Ye line batati hai ki hum JWT authentication use karenge.

.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});
// Ye JWT token ko validate karne ke rules set karta hai, jaise ki issuer, audience, aur signing key.

// Database Configuration (Fixed Migration Assembly)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("FunDooNotesC_.DataLayer") // Corrected migration assembly
    ));
// Ye line database connection setup karta hai aur batata hai ki migrations (database updates) ka code kahan hai.

// Dependency Injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// Ye line generic repository ko register karta hai. Isse hum kisi bhi entity ke liye repository use kar sakte hain.

builder.Services.AddScoped<IUserRepository, UserRepository>();
// Ye line UserRepository ko register karta hai. Isse User-related operations handle hote hain.

builder.Services.AddScoped<IUserService, UserService>();
// Ye line UserService ko register karta hai. Isse User-related business logic handle hoti hai.

builder.Services.AddScoped<INoteService, NoteService>();
// Ye line NoteService ko register karta hai. Isse Note-related business logic handle hoti hai.

var app = builder.Build();
// Ye line app ko build karta hai. Iske baad hum middleware aur routing setup karte hain.

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Ye lines Swagger ko enable karti hain, but sirf development environment mein.

app.UseHttpsRedirection();
// Ye line HTTP requests ko HTTPS mein redirect karti hai for security.

app.UseAuthentication();
// Ye line authentication middleware ko enable karti hai. Isse JWT tokens validate hote hain.

app.UseAuthorization();
// Ye line authorization middleware ko enable karti hai. Isse batata hai ki user ke paas kya permissions hain.

app.MapControllers();
// Ye line controllers ko map karti hai. Isse API endpoints kaam karte hain.

app.Run();
// Ye line app ko run karti hai. Iske baad app start ho jata hai aur requests sunne lagta hai.