using FunDooNotesC_.BusinessLogicLayer.Interfaces; // Interface ka use jo note-related logic ko handle karega
using FunDooNotesC_.DataLayer.Entities; // Notes se related entity ko access karne ke liye
using Microsoft.AspNetCore.Mvc; // API controller banane ke liye
using System.Threading.Tasks; // Asynchronous operations ko handle karne ke liye

namespace FunDooNotesC_.BusinessLayer.Controllers // Namespace define kar raha hai jo is controller ko identify karega
{
    // Ye controller notes se related API requests handle karega
    [ApiController] // Controller ko API controller ke roop me mark kar raha hai
    [Route("api/[controller]")] // API ka base route set kar raha hai, e.g., api/notes
    public class NotesController : ControllerBase // NotesController class jo ControllerBase inherit karega
    {
        private readonly INoteService _noteService; // INoteService ka reference jo note-related business logic handle karega

        // Constructor jo NoteService ko inject karega (Dependency Injection)
        public NotesController(INoteService noteService)
        {
            _noteService = noteService; // Private field me service ko assign kar rahe hain
        }

        // Get API (saare notes retrieve karne ke liye)
        [HttpGet] // HTTP GET request handle karega (URL: api/notes)
        public async Task<IActionResult> GetAll()
        {
            var notes = await _noteService.GetAllNotesAsync(); // Saare notes ko fetch kar raha hai
            return Ok(notes); // Notes ko response me bhej raha hai
        }

        // Get API (ek particular note retrieve karne ke liye ID ke basis par)
        [HttpGet("{id}")] // ID ke saath HTTP GET request handle karega (URL: api/notes/{id})
        public async Task<IActionResult> GetById(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id); // ID ke basis par note fetch kar raha hai
            if (note == null) // Agar note nahi mila to
                return NotFound(); // 404 response dega

            return Ok(note); // Note return karega agar mila toh
        }

        // Post API (naya note create karne ke liye)
        [HttpPost] // HTTP POST request handle karega (URL: api/notes)
        public async Task<IActionResult> Create([FromBody] Note note) // Request body se note ka data lega
        {
            var createdNote = await _noteService.CreateNoteAsync(note); // Naya note create kar raha hai
            return CreatedAtAction(nameof(GetById), new { id = createdNote.Id }, createdNote); // Response me naye note ka ID return karega
        }

        // Put API (existing note update karne ke liye)
        [HttpPut("{id}")] // HTTP PUT request handle karega (URL: api/notes/{id})
        public async Task<IActionResult> Update(int id, [FromBody] Note note) // Request body me updated note ka data lega
        {
            if (id != note.Id) // Agar provided ID aur note ki ID match nahi karti toh
                return BadRequest("Note ID mismatch"); // Bad request response bhejega

            await _noteService.UpdateNoteAsync(note); // Note ko update karega
            return NoContent(); // Successfully update hone ke baad empty response dega
        }

        // Delete API (ek note delete karne ke liye)
        [HttpDelete("{id}")] // HTTP DELETE request handle karega (URL: api/notes/{id})
        public async Task<IActionResult> Delete(int id)
        {
            await _noteService.DeleteNoteAsync(id); // Note ko delete karega
            return NoContent(); // Successfully delete hone ke baad empty response dega
        }

    }
}
