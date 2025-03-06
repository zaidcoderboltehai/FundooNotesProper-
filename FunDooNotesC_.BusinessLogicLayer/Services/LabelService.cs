using FunDooNotesC_.BusinessLogicLayer.Interfaces;
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.RepoLayer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using FunDooNotesC_.BusinessLogicLayer.DTOs;

namespace FunDooNotesC_.BusinessLogicLayer.Services
{
    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepo;
        private readonly IRepository<NoteLabel> _noteLabelRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LabelService(
            ILabelRepository labelRepo,
            IRepository<NoteLabel> noteLabelRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _labelRepo = labelRepo;
            _noteLabelRepo = noteLabelRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // Changed parameter to DTO
        public async Task<Label> CreateLabel(LabelCreateDTO labelDto)
        {
            // Get User ID from JWT token
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create a new Label object from DTO
            var label = new Label
            {
                Name = labelDto.Name,
                UserId = int.Parse(userId) // Auto-set UserId
            };

            // Check for duplicate label name
            var existing = await _labelRepo.GetAllAsync(l =>
                l.UserId == label.UserId &&
                l.Name == label.Name);

            if (existing.Any())
                throw new System.Exception("Label name already exists.");

            await _labelRepo.AddAsync(label);
            return label;
        }

        // Keep other methods unchanged
        public async Task<IEnumerable<Label>> GetUserLabels(int userId) =>
            await _labelRepo.GetLabelsByUser(userId);

        public async Task DeleteLabel(int labelId) =>
            await _labelRepo.DeleteAsync(labelId);

        public async Task AddLabelToNote(int noteId, int labelId) =>
            await _noteLabelRepo.AddAsync(new NoteLabel { NoteId = noteId, LabelId = labelId });

        public async Task RemoveLabelFromNote(int noteId, int labelId)
        {
            var noteLabels = await _noteLabelRepo.GetAllAsync(nl =>
                nl.NoteId == noteId &&
                nl.LabelId == labelId);

            foreach (var noteLabel in noteLabels)
                await _noteLabelRepo.DeleteAsync(noteLabel);
        }

        public async Task<Label> GetLabelByIdAsync(int labelId) =>
            await _labelRepo.GetByIdAsync(labelId);
        public async Task UpdateLabelAsync(Label label)
        {
            // Add validation if needed
            await _labelRepo.UpdateAsync(label);
        }

    }
}