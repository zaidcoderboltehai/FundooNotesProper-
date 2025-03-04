// FunDooNotesC_.BusinessLogicLayer/Services/NoteService.cs
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.RepoLayer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLogicLayer.Services
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository;

        public NoteService(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<IEnumerable<Note>> GetUserNotesAsync(int userId)
        {
            return await _noteRepository.GetAllAsync(n =>
                n.UserId == userId &&
                !n.IsArchived &&
                !n.IsTrashed
            );
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            return await _noteRepository.GetByIdAsync(id);
        }

        public async Task<Note> CreateNoteAsync(Note note)
        {
            await _noteRepository.AddAsync(note);
            return note;
        }

        public async Task UpdateNoteAsync(Note note)
        {
            await _noteRepository.UpdateAsync(note);
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note != null)
            {
                note.IsTrashed = true;
                note.DeletedDate = DateTime.UtcNow;
                await _noteRepository.UpdateAsync(note);
            }
        }

        // Archive/Trash Implementation
        public async Task ArchiveNoteAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note != null)
            {
                note.IsArchived = true;
                await _noteRepository.UpdateAsync(note);
            }
        }

        public async Task UnarchiveNoteAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note != null)
            {
                note.IsArchived = false;
                await _noteRepository.UpdateAsync(note);
            }
        }

        public async Task TrashNoteAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note != null)
            {
                note.IsTrashed = true;
                note.DeletedDate = DateTime.UtcNow;
                await _noteRepository.UpdateAsync(note);
            }
        }

        public async Task RestoreNoteAsync(int id)
        {
            var note = await _noteRepository.GetByIdAsync(id);
            if (note != null)
            {
                note.IsTrashed = false;
                note.DeletedDate = null;
                await _noteRepository.UpdateAsync(note);
            }
        }

        public async Task DeletePermanentlyAsync(int id)
        {
            await _noteRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Note>> GetArchivedNotesAsync(int userId)
        {
            return await _noteRepository.GetAllAsync(n =>
                n.UserId == userId &&
                n.IsArchived &&
                !n.IsTrashed
            );
        }

        public async Task<IEnumerable<Note>> GetTrashedNotesAsync(int userId)
        {
            return await _noteRepository.GetAllAsync(n =>
                n.UserId == userId &&
                n.IsTrashed
            );
        }
    }
}