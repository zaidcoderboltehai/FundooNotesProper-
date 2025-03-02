using FunDooNotesC_.BusinessLogicLayer.Interfaces; // Note service ke interface ko use kar raha hai
using FunDooNotesC_.DataLayer.Entities;           // Note entity ka reference le raha hai
using FunDooNotesC_.RepoLayer;                     // Repository classes ko include kar raha hai
using System.Collections.Generic;                  // List aur collections ke liye
using System.Threading.Tasks;                      // Asynchronous programming ke liye

namespace FunDooNotesC_.BusinessLogicLayer.Services // Yeh class Services namespace me hai
{
    // NoteService class jo INoteService interface ko implement kar rahi hai
    public class NoteService : INoteService
    {
        private readonly IRepository<Note> _noteRepository; // Repository ka private instance jo database se interact karega

        // Constructor: jab NoteService class ka object banega toh repository ka instance milega
        public NoteService(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository; // Repository ko assign kar raha hai
        }

        // Saare notes ko async tareeke se fetch karega
        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _noteRepository.GetAllAsync(); // Repository se sabhi notes la raha hai
        }

        // Ek specific note ko ID ke basis par fetch karega
        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            return await _noteRepository.GetByIdAsync(id); // Diye gaye ID se note return karega
        }

        // Naya note create karega aur async tareeke se database me store karega
        public async Task<Note> CreateNoteAsync(Note note)
        {
            await _noteRepository.AddAsync(note); // Naya note database me insert kar raha hai
            return note; // Inserted note ko return kar raha hai
        }

        // Pehle se maujood note ko update karega
        public async Task UpdateNoteAsync(Note note)
        {
            await _noteRepository.UpdateAsync(note); // Note ko repository ke through update kar raha hai
        }

        // Diye gaye ID wale note ko database se delete karega
        public async Task DeleteNoteAsync(int id)
        {
            await _noteRepository.DeleteAsync(id); // Note ko async tareeke se delete kar raha hai
        }
    }
}
