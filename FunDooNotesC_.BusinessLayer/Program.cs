using FunDooNotesC_.DataLayer;
using FunDooNotesC_.RepoLayer;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.BusinessLogicLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using FunDooNotesC_.DataLayer.Entities;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

/***************************** Services Configuration *****************************/
// 1. Logging Setup
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// 2. Core Services
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// 3. Register Repositories and Services
builder.Services.AddScoped<IUserRepository>(provider =>
    new UserRepository(
        provider.GetRequiredService<ApplicationDbContext>(),
        provider.GetRequiredService<IConnectionMultiplexer>()
    ));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRepository<Note>, Repository<Note>>();
builder.Services.AddScoped<IRepository<NoteLabel>, Repository<NoteLabel>>();
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<ILabelRepository, LabelRepository>();
builder.Services.AddScoped<ILabelService, LabelService>();

// 4. Swagger Configuration (with JWT)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FunDooNotes API",
        Version = "v1"
    });

    // JWT Support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

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
});

// 5. JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

// 6. Database Configuration (Updated)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b =>
        {
            b.MigrationsAssembly("FunDooNotesC_.DataLayer");
            b.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        }
    ));

// 7. Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")
    ));

// 8. CORS Configuration (Allow Angular app)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

/***************************** App Building *****************************/
var app = builder.Build();

// Middleware Order
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Fix: Move UseRouting before authentication and authorization
app.UseRouting();

// CORS must come after Routing but before Authentication
app.UseCors("AllowAngularApp");

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Finally, map endpoints
app.MapControllers();

app.Run();
