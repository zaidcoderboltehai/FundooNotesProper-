using Microsoft.EntityFrameworkCore;
using FunDooNotesC_.DataLayer.Entities;

namespace FunDooNotesC_.DataLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Database Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between User and Note
            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)          // A Note belongs to one User
                .WithMany(u => u.Notes)       // A User can have many Notes
                .HasForeignKey(n => n.UserId) // Foreign key in Notes table
                .OnDelete(DeleteBehavior.Cascade); // Optional: Delete notes when user is deleted
        }
    }
}