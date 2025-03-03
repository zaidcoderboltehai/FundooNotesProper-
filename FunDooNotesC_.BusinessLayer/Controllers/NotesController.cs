// Controllers/NotesController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace FunDooNotesC_.BusinessLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            var notes = await _noteService.GetUserNotesAsync(userId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.UserId != GetCurrentUserId())
                return NotFound(new { error = "Note not found" });

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteRequest request)
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

            var createdNote = await _noteService.CreateNoteAsync(note);
            return CreatedAtAction(nameof(GetById), new { id = createdNote.Id }, createdNote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NoteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingNote = await _noteService.GetNoteByIdAsync(id);
            if (existingNote == null || existingNote.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            existingNote.Title = request.Title;
            existingNote.Description = request.Description;
            existingNote.Color = request.Color;

            await _noteService.UpdateNoteAsync(existingNote);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null || note.UserId != GetCurrentUserId())
                return Unauthorized(new { error = "Unauthorized access" });

            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
    }

    public class NoteRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [StringLength(20, ErrorMessage = "Color code cannot exceed 20 characters")]
        public string Color { get; set; }
    }
}