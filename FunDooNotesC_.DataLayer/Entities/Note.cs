using System.ComponentModel.DataAnnotations;
using FunDooNotesC_.DataLayer.Entities; // User entity ka reference add karo

namespace FunDooNotesC_.DataLayer.Entities
{
    public class Note
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(20)]
        public string Color { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; } // Foreign Key

        // Navigation Property (Yeh add karna zaroori hai)
        public User User { get; set; }
    }
}