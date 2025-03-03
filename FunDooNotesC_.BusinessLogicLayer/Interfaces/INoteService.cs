using FunDooNotesC_.DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces
{
    public interface INoteService
    {
        // New method to get notes based on userId
        Task<IEnumerable<Note>> GetUserNotesAsync(int userId);
        Task<Note?> GetNoteByIdAsync(int id);
        Task<Note> CreateNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
        Task DeleteNoteAsync(int id);
    }
}
