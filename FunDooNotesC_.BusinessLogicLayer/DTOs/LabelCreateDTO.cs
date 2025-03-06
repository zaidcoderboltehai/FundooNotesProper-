// FunDooNotesC_.BusinessLogicLayer/DTOs/LabelCreateDTO.cs
using System.ComponentModel.DataAnnotations;

namespace FunDooNotesC_.BusinessLogicLayer.DTOs
{
    public class LabelCreateDTO
    {
        [Required(ErrorMessage = "Label name is required")]
        public string Name { get; set; }
    }
}