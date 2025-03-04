using Microsoft.EntityFrameworkCore; // Entity Framework Core ka use kar rahe hain database operations ke liye.
using FunDooNotesC_.DataLayer.Entities; // Entities (User, Note) ka reference include kar rahe hain.

namespace FunDooNotesC_.DataLayer // Namespace jisme database context define kiya gaya hai.
{
    public class ApplicationDbContext : DbContext // DbContext inherit karke database se interact karne wala class bana rahe hain.
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) // Constructor jo options leta hai.
            : base(options) // Parent class (DbContext) ke constructor ko options pass kar rahe hain.
        { }

        // Database Tables
        public DbSet<User> Users { get; set; } // Users table represent kar raha hai.
        public DbSet<Note> Notes { get; set; } // Notes table represent kar raha hai.

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Database relationships configure karne ke liye.
        {
            base.OnModelCreating(modelBuilder); // Parent class ka OnModelCreating method call kar rahe hain.

            // Configure the relationship between User and Note
            modelBuilder.Entity<Note>() // Note entity ke liye relationship set kar rahe hain.
                .HasOne(n => n.User)          // Ek Note ek User se belong karega (one-to-many relationship).
                .WithMany(u => u.Notes)       // Ek User ke multiple Notes ho sakte hain.
                .HasForeignKey(n => n.UserId) // Notes table me UserId foreign key hoga.
                .OnDelete(DeleteBehavior.Cascade); // Agar User delete ho to uske saare Notes bhi delete ho jayein.
        }
    }
}
