using Microsoft.EntityFrameworkCore; // Entity Framework Core ka use kar rahe hain database interaction ke liye
using FunDooNotesC_.DataLayer.Entities; // Entities ko include kar rahe hain, jo database tables ko represent karti hain

namespace FunDooNotesC_.DataLayer
{
    // ApplicationDbContext ek DbContext class hai jo database connection ko handle karega
    public class ApplicationDbContext : DbContext
    {
        // Constructor jo database connection ke options ko initialize karega
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Base class DbContext ko options pass kar rahe hain
        { }

        // Users table ko represent karega
        public DbSet<User> Users => Set<User>();

        // Notes table ko represent karega
        public DbSet<Note> Notes => Set<Note>();

        // Database ke models ka configuration yahan define kar sakte hain
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Parent class ka OnModelCreating method call kar rahe hain
        }
    }
}
