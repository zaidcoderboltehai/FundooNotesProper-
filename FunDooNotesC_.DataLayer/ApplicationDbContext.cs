using Microsoft.EntityFrameworkCore;
using FunDooNotesC_.DataLayer.Entities;

namespace FunDooNotesC_.DataLayer
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Note> Notes { get; set; } = null!;
        public DbSet<Label> Labels { get; set; } = null!;
        public DbSet<NoteLabel> NoteLabels { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the composite key for NoteLabel
            modelBuilder.Entity<NoteLabel>().HasKey(nl => new { nl.NoteId, nl.LabelId });

            // Configure the relationship between NoteLabel and Note
            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Note)
                .WithMany(n => n.NoteLabels)
                .HasForeignKey(nl => nl.NoteId)
                .OnDelete(DeleteBehavior.NoAction); // Changed to NoAction to avoid cascade cycles

            // Configure the relationship between NoteLabel and Label
            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Label)
                .WithMany(l => l.NoteLabels)
                .HasForeignKey(nl => nl.LabelId)
                .OnDelete(DeleteBehavior.NoAction); // Changed to NoAction to avoid cascade cycles

            // Configure the relationship between Note and User
            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Notes when User is deleted
        }
    }
}