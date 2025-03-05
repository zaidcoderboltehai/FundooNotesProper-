using System.Collections.Generic;
using System.Threading.Tasks;
using FunDooNotesC_.DataLayer.Entities;

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces
{
    public interface INoteService
    {
        // Fetch all notes for a specific user (excluding archived and trashed notes)
        Task<IEnumerable<Note>> GetUserNotesAsync(int userId);

        // Fetch a specific note by its ID
        Task<Note?> GetNoteByIdAsync(int id);

        // Create a new note
        Task<Note> CreateNoteAsync(Note note);

        // Update an existing note
        Task UpdateNoteAsync(Note note);

        // Soft delete a note (mark as trashed)
        Task DeleteNoteAsync(int id);

        // Archive a note
        Task ArchiveNoteAsync(int id);

        // Unarchive a note
        Task UnarchiveNoteAsync(int id);

        // Move a note to the trash
        Task TrashNoteAsync(int id);

        // Restore a note from the trash
        Task RestoreNoteAsync(int id);

        // Permanently delete a note and its associated NoteLabel records
        Task DeletePermanentlyAsync(int id);

        // Fetch all archived notes for a specific user
        Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId);

        // Fetch all trashed notes for a specific user
        Task<IEnumerable<Note>> GetTrashedNotesAsync(int userId);
    }
}