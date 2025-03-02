// FunDooNotes_App.DAL/Entities/Note.cs

using System; // Basic system functionalities ke liye

namespace FunDooNotesC_.DataLayer.Entities
{
    // Note class ek model hai jo ek note ka data store karega
    public class Note
    {
        // Har ek note ka unique ID hoga (Primary Key in database)
        public int Id { get; set; }

        // Note ka title store karega, default empty string hai taaki null na ho
        public string Title { get; set; } = string.Empty;

        // Note ka actual content store karega, yeh bhi empty string hai default me
        public string Content { get; set; } = string.Empty;

        // Note kab create hua, iska timestamp store karega (default UTC time rakha hai)
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // User ka ID store karega jo batayega ki yeh note kis user ka hai (Foreign Key)
        public int UserId { get; set; }

        // Navigation property jo User entity se connect karega (database relationship establish karega)
        public User? User { get; set; }
    }
}
