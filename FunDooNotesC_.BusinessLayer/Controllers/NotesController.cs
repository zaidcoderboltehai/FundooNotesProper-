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
        private readonly ILabelService _labelService;
        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public NotesController(INoteService noteService, ILabelService labelService)
        {
            _noteService = noteService;
            _labelService = labelService;
        }

        // ... Existing methods (CreateNote, GetNotes, Archive, etc.) ...

        // ------------------------ LABEL LINKING ------------------------
        [HttpPost("{noteId}/labels/{labelId}")]
        public async Task<IActionResult> AddLabelToNote(int noteId, int labelId)
        {
            // Check note ownership
            var note = await _noteService.GetNoteByIdAsync(noteId);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            // Check label ownership
            var label = await _labelService.GetLabelByIdAsync(labelId);
            if (label == null || label.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _labelService.AddLabelToNote(noteId, labelId);
            return Ok();
        }

        [HttpDelete("{noteId}/labels/{labelId}")]
        public async Task<IActionResult> RemoveLabelFromNote(int noteId, int labelId)
        {
            // Check note ownership
            var note = await _noteService.GetNoteByIdAsync(noteId);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            // Check label ownership
            var label = await _labelService.GetLabelByIdAsync(labelId);
            if (label == null || label.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _labelService.RemoveLabelFromNote(noteId, labelId);
            return NoContent();
        }
    }

    public class NoteRequest { /* ... Existing code ... */ }
}