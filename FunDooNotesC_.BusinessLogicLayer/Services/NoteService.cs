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

        // Option 1: Filtering in the Service Layer
        public async Task<IEnumerable<Note>> GetUserNotesAsync(int userId)
        {
            // Fetch all notes using the generic GetAllAsync() method from the repository.
            var allNotes = await _noteRepository.GetAllAsync();
            // Filter using LINQ to return only those notes that belong to the specified user.
            return allNotes.Where(n => n.UserId == userId);
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
            await _noteRepository.DeleteAsync(id);
        }
    }
}
