using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.BusinessLogicLayer.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FunDooNotesC_.BusinessLayer.Controllers
{
    [ApiController]
    [Route("api/notes")]
    [Authorize] // Ensure this is present
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly ILabelService _labelService;
        private readonly ILogger<NotesController> _logger;
        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public NotesController(
            INoteService noteService,
            ILabelService labelService,
            ILogger<NotesController> logger)
        {
            _noteService = noteService;
            _labelService = labelService;
            _logger = logger;
        }

        // ------------------------ GET USER NOTES ------------------------
        [HttpGet]
        public async Task<IActionResult> GetUserNotes()
        {
            try
            {
                var notes = await _noteService.GetUserNotesAsync(GetCurrentUserId());
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching notes");
                return BadRequest(new { error = "Failed to get notes" });
            }
        }

        // ------------------------ CREATE NOTE ------------------------
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var note = new Note
                {
                    Title = request.Title,
                    Description = request.Description,
                    Color = request.Color,
                    UserId = GetCurrentUserId()
                };

                var createdNote = await _noteService.CreateNoteAsync(note);
                return Ok(createdNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating note");
                return BadRequest(new { error = "Note creation failed" });
            }
        }

        // ------------------------ UPDATE NOTE ------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] NoteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingNote = await _noteService.GetNoteByIdAsync(id);
                if (existingNote == null || existingNote.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                existingNote.Title = request.Title;
                existingNote.Description = request.Description;
                existingNote.Color = request.Color;

                await _noteService.UpdateNoteAsync(existingNote);
                return Ok(existingNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating note {id}");
                return BadRequest(new { error = "Note update failed" });
            }
        }

        // ------------------------ SOFT DELETE NOTE ------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                if (note == null || note.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                await _noteService.DeleteNoteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting note {id}");
                return BadRequest(new { error = "Note deletion failed" });
            }
        }

        // ------------------------ GET ARCHIVED NOTES ------------------------
        [HttpGet("archived")]
        public async Task<IActionResult> GetArchivedNotes()
        {
            try
            {
                var notes = await _noteService.GetArchivedNotesAsync(GetCurrentUserId());
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching archived notes");
                return BadRequest(new { error = "Failed to get archived notes" });
            }
        }

        // ------------------------ ARCHIVE/UNARCHIVE NOTE ------------------------
        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ArchiveNote(int id, [FromQuery] bool archive = true)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                if (note == null || note.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                if (archive)
                    await _noteService.ArchiveNoteAsync(id);
                else
                    await _noteService.UnarchiveNoteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error {(archive ? "archiving" : "unarchiving")} note {id}");
                return BadRequest(new { error = $"Note {(archive ? "archive" : "unarchive")} failed" });
            }
        }

        // ------------------------ GET TRASHED NOTES ------------------------
        [HttpGet("trashed")]
        public async Task<IActionResult> GetTrashedNotes()
        {
            try
            {
                var notes = await _noteService.GetTrashedNotesAsync(GetCurrentUserId());
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching trashed notes");
                return BadRequest(new { error = "Failed to get trashed notes" });
            }
        }

        // ------------------------ TRASH/RESTORE NOTE ------------------------
        [HttpPatch("{id}/trash")]
        public async Task<IActionResult> TrashNote(int id, [FromQuery] bool trash = true)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                if (note == null || note.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                if (trash)
                    await _noteService.TrashNoteAsync(id);
                else
                    await _noteService.RestoreNoteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error {(trash ? "trashing" : "restoring")} note {id}");
                return BadRequest(new { error = $"Note {(trash ? "trash" : "restore")} failed" });
            }
        }

        // ------------------------ LABEL LINKING ------------------------
        [HttpPost("{noteId}/labels/{labelId}")]
        public async Task<IActionResult> AddLabelToNote(int noteId, int labelId)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(noteId);
                if (note == null || note.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                var label = await _labelService.GetLabelByIdAsync(labelId);
                if (label == null || label.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                await _labelService.AddLabelToNote(noteId, labelId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding label {labelId} to note {noteId}");
                return BadRequest(new { error = "Label linking failed" });
            }
        }

        [HttpDelete("{noteId}/labels/{labelId}")]
        public async Task<IActionResult> RemoveLabelFromNote(int noteId, int labelId)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(noteId);
                if (note == null || note.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                var label = await _labelService.GetLabelByIdAsync(labelId);
                if (label == null || label.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                await _labelService.RemoveLabelFromNote(noteId, labelId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing label {labelId} from note {noteId}");
                return BadRequest(new { error = "Label removal failed" });
            }
        }
    }
}