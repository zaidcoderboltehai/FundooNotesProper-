using FunDooNotesC_.DataLayer.Entities; // Updated namespace, jo Note entity ko use karega
using System.Collections.Generic; // Yeh List (IEnumerable) wali functionality lane ke liye hai
using System.Threading.Tasks; // Yeh asynchronous methods ko support karne ke liye hai

namespace FunDooNotesC_.BusinessLogicLayer.Interfaces // Business logic layer ke interfaces ka namespace define kar raha hai
{
    public interface INoteService // INoteService naam ka ek interface bana raha hai
    {
        Task<IEnumerable<Note>> GetAllNotesAsync(); // Yeh method saare notes ko fetch karega asynchronously

        Task<Note?> GetNoteByIdAsync(int id); // Yeh method ek particular note ko fetch karega ID ke basis par asynchronously

        Task<Note> CreateNoteAsync(Note note); // Yeh method ek naya note create karega asynchronously

        Task UpdateNoteAsync(Note note); // Yeh method ek existing note ko update karega asynchronously

        Task DeleteNoteAsync(int id); // Yeh method ek note ko ID ke basis par delete karega asynchronously
    }
}
