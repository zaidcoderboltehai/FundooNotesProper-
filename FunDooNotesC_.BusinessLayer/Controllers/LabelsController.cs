using Microsoft.AspNetCore.Mvc;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FunDooNotesC_.BusinessLogicLayer.DTOs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace FunDooNotesC_.BusinessLayer.Controllers
{
    [ApiController]
    [Route("api/labels")]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly ILogger<LabelsController> _logger;
        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        public LabelsController(
            ILabelService labelService,
            ILogger<LabelsController> logger)
        {
            _labelService = labelService;
            _logger = logger;
        }

        // ------------------------ CREATE LABEL ------------------------
        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody] LabelCreateDTO labelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdLabel = await _labelService.CreateLabel(labelDto);
                return Ok(createdLabel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating label");
                return BadRequest(new { error = "Label creation failed" });
            }
        }

        // ------------------------ UPDATE LABEL ------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabel(int id, [FromBody] LabelUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var label = await _labelService.GetLabelByIdAsync(id);

                // Authorization check
                if (label == null || label.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                // Check for duplicate names
                var existing = await _labelService.GetUserLabels(GetCurrentUserId());
                if (existing.Any(l => l.Name == dto.Name && l.Id != id))
                    return Conflict(new { error = "Label name already exists" });

                label.Name = dto.Name;
                await _labelService.UpdateLabelAsync(label);

                return Ok(label);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating label {id}");
                return BadRequest(new { error = "Label update failed" });
            }
        }

        // ------------------------ GET ALL LABELS ------------------------
        [HttpGet]
        public async Task<IActionResult> GetUserLabels()
        {
            try
            {
                var labels = await _labelService.GetUserLabels(GetCurrentUserId());
                return Ok(labels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching labels");
                return BadRequest(new { error = "Failed to get labels" });
            }
        }

        // ------------------------ DELETE LABEL ------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            try
            {
                var label = await _labelService.GetLabelByIdAsync(id);
                if (label == null || label.UserId != GetCurrentUserId())
                    return Unauthorized(new { error = "Unauthorized access" });

                await _labelService.DeleteLabel(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting label {id}");
                return BadRequest(new { error = "Label deletion failed" });
            }
        }
    }

    // Add to DTOs folder
    public class LabelUpdateDTO
    {
        [Required(ErrorMessage = "Label name is required")]
        [StringLength(50, ErrorMessage = "Label name cannot exceed 50 characters")]
        public string Name { get; set; }
    }
}