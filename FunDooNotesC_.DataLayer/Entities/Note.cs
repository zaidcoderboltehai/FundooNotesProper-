// FunDooNotesC_.DataLayer/Entities/Note.cs
using System.ComponentModel.DataAnnotations;

namespace FunDooNotesC_.DataLayer.Entities
{
    public class Note
    {
        // Add this line at the bottom of existing Note class
        public ICollection<NoteLabel> NoteLabels { get; set; }
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
        public int UserId { get; set; }
        public bool IsArchived { get; set; } = false;
        public bool IsTrashed { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        public User User { get; set; }
    }
}