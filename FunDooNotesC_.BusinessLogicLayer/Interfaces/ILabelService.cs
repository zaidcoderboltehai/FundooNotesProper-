// FunDooNotesC_.BusinessLogicLayer/Interfaces/ILabelService.cs
using FunDooNotesC_.BusinessLogicLayer.DTOs;
using FunDooNotesC_.DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces
{
    public interface ILabelService
    {
        // Change parameter type from Label to LabelCreateDTO
        Task<Label> CreateLabel(LabelCreateDTO labelDto);

        Task<IEnumerable<Label>> GetUserLabels(int userId);
        Task DeleteLabel(int labelId);
        Task AddLabelToNote(int noteId, int labelId);
        Task RemoveLabelFromNote(int noteId, int labelId);
        Task<Label> GetLabelByIdAsync(int labelId);
        Task UpdateLabelAsync(Label label);

    }
}