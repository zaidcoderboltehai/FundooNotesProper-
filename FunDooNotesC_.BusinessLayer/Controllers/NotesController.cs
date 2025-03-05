using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunDooNotesC_.BusinessLayer.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;
        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // ------------------------ CREATE NOTE ------------------------
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var note = new Note
            {
                Title = request.Title,
                Description = request.Description,
                Color = request.Color,
                UserId = GetCurrentUserId(),
                CreatedDate = DateTime.UtcNow
            };

            await _noteService.CreateNoteAsync(note);
            return Ok(new
            {
                note.Id,
                note.Title,
                note.Description,
                note.Color
            });
        }

        // ------------------------ GET ALL NOTES ------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var userId = GetCurrentUserId();
            var notes = await _noteService.GetUserNotesAsync(userId);
            return Ok(notes);
        }

        // ------------------------ ARCHIVE/UNARCHIVE ------------------------
        [HttpPut("{id}/archive")]
        public async Task<IActionResult> Archive(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _noteService.ArchiveNoteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/unarchive")]
        public async Task<IActionResult> Unarchive(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _noteService.UnarchiveNoteAsync(id);
            return NoContent();
        }

        // ------------------------ TRASH/RESTORE ------------------------
        [HttpPut("{id}/trash")]
        public async Task<IActionResult> Trash(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _noteService.TrashNoteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _noteService.RestoreNoteAsync(id);
            return NoContent();
        }

        // ------------------------ DELETE ------------------------
        [HttpDelete("{id}/permanent")]
        public async Task<IActionResult> DeletePermanently(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _noteService.DeletePermanentlyAsync(id);
            return NoContent();
        }

        // ------------------------ GET ARCHIVED/TRASHED ------------------------
        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedNotes()
        {
            var userId = GetCurrentUserId();
            return Ok(await _noteService.GetArchivedNotesAsync(userId));
        }

        [HttpGet("trashed")]
        public async Task<IActionResult> GetTrashedNotes()
        {
            var userId = GetCurrentUserId();
            return Ok(await _noteService.GetTrashedNotesAsync(userId));
        }
    }

    public class NoteRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Color code cannot exceed 20 characters")]
        public string Color { get; set; } = string.Empty;
    }
}