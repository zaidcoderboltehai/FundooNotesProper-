// FunDooNotesC_.BusinessLogicLayer/Interfaces/INoteService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using FunDooNotesC_.DataLayer.Entities;

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetUserNotesAsync(int userId);
        Task<Note?> GetNoteByIdAsync(int id);
        Task<Note> CreateNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
        Task DeleteNoteAsync(int id);

        // New methods
        Task ArchiveNoteAsync(int id);
        Task UnarchiveNoteAsync(int id);
        Task TrashNoteAsync(int id);
        Task RestoreNoteAsync(int id);
        Task DeletePermanentlyAsync(int id);
        Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId);
        Task<IEnumerable<Note>> GetTrashedNotesAsync(int userId);
    }
}