using System.ComponentModel.DataAnnotations;

namespace FunDooNotesC_.BusinessLogicLayer.DTOs
{
    public class NoteRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string Color { get; set; }
    }
}