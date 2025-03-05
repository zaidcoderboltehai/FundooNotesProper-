using FunDooNotesC_.DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces
{
    public interface ILabelService
    {
        Task<Label> CreateLabel(Label label);
        Task<IEnumerable<Label>> GetUserLabels(int userId);
        Task DeleteLabel(int labelId);
        Task AddLabelToNote(int noteId, int labelId);
        Task RemoveLabelFromNote(int noteId, int labelId);
        Task<Label> GetLabelByIdAsync(int labelId); // Added this line
    }
}