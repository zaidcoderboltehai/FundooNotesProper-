using FunDooNotesC_.DataLayer;                   // Data layer include kar raha hai (pehle iska naam FunDooNotes_App.DAL tha)
using FunDooNotesC_.RepoLayer;                    // Repository layer include kar raha hai (pehle FunDooNotes_App.DAL.Repositories tha)
using FunDooNotesC_.BusinessLogicLayer.Interfaces; // Business logic ke interfaces include kar raha hai (pehle FunDooNotes_App.BLL.Interfaces tha)
using FunDooNotesC_.BusinessLogicLayer.Services;   // Business logic ke services include kar raha hai (pehle FunDooNotes_App.BLL.Services tha)
using Microsoft.EntityFrameworkCore;              // Entity Framework Core (EF Core) ko use kar raha hai database connection ke liye

var builder = WebApplication.CreateBuilder(args); // Web application ka builder create kar raha hai

// Services ko application ke container me add kar raha hai
builder.Services.AddControllers(); // Controllers ko enable kar raha hai
builder.Services.AddEndpointsApiExplorer(); // API endpoints ke liye explorer enable kar raha hai
builder.Services.AddSwaggerGen(); // Swagger documentation enable kar raha hai

// EF Core ko SQL Server ke sath configure kar raha hai (Permanent database ke liye)
// Ensure kar raha hai ki sahi connection string appsettings.json me set ho
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), // Connection string le raha hai
        sqlOptions => sqlOptions.MigrationsAssembly("FunDooNotes_App.WebAPI") // Migrations ke liye WebAPI project specify kar raha hai
    ));

// Repository services ko register kar raha hai (Dependency Injection ke liye)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Generic repository register kar raha hai
builder.Services.AddScoped<IUserRepository, UserRepository>(); // User repository ko register kar raha hai

// Business logic layer (BLL) services ko register kar raha hai
builder.Services.AddScoped<IUserService, UserService>(); // User service ko register kar raha hai
builder.Services.AddScoped<INoteService, NoteService>(); // Note service ko register kar raha hai

var app = builder.Build(); // Application build kar raha hai

// HTTP request pipeline ko configure kar raha hai
if (app.Environment.IsDevelopment()) // Agar development mode hai toh
{
    app.UseSwagger(); // Swagger ko enable kar raha hai
    app.UseSwaggerUI(); // Swagger UI ko enable kar raha hai
}

app.UseHttpsRedirection(); // HTTPS redirection enable kar raha hai
app.UseAuthorization(); // Authorization middleware use kar raha hai
app.MapControllers(); // Controllers ko map kar raha hai taki API requests handle ho sake
app.Run(); // Application ko run kar raha hai
