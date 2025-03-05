using Microsoft.AspNetCore.Mvc;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/labels")]
[Authorize]
public class LabelsController : ControllerBase
{
    private readonly ILabelService _labelService;
    public LabelsController(ILabelService labelService) => _labelService = labelService;

    private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpPost]
    public async Task<IActionResult> CreateLabel([FromBody] Label label)
    {
        try
        {
            label.UserId = GetCurrentUserId();
            var createdLabel = await _labelService.CreateLabel(label);
            return Ok(createdLabel);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetUserLabels()
    {
        var userId = GetCurrentUserId();
        return Ok(await _labelService.GetUserLabels(userId));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLabel(int id)
    {
        var label = await _labelService.GetLabelByIdAsync(id);
        if (label == null || label.UserId != GetCurrentUserId())
            return Unauthorized(new { error = "Unauthorized access" });

        await _labelService.DeleteLabel(id);
        return NoContent();
    }
}