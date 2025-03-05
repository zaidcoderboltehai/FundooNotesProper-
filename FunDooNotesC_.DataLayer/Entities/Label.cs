// FunDooNotesC_.DataLayer/Entities/Label.cs
namespace FunDooNotesC_.DataLayer.Entities
{
    public class Label
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<NoteLabel> NoteLabels { get; set; } = new List<NoteLabel>(); // Initialize here
    }
}