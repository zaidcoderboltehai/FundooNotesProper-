using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.RepoLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FunDooNotesC_.BusinessLogicLayer.Interfaces;

namespace FunDooNotesC_.BusinessLogicLayer.Services
{
    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepo;
        private readonly IRepository<NoteLabel> _noteLabelRepo;

        public LabelService(ILabelRepository labelRepo, IRepository<NoteLabel> noteLabelRepo)
        {
            _labelRepo = labelRepo;
            _noteLabelRepo = noteLabelRepo;
        }

        public async Task<Label> CreateLabel(Label label)
        {
            var existing = await _labelRepo.GetAllAsync(l =>
                l.UserId == label.UserId && l.Name == label.Name);
            if (existing.Any())
                throw new System.Exception("Label name already exists.");

            await _labelRepo.AddAsync(label);
            return label;
        }

        public async Task<IEnumerable<Label>> GetUserLabels(int userId)
        {
            return await _labelRepo.GetLabelsByUser(userId);
        }

        public async Task DeleteLabel(int labelId)
        {
            await _labelRepo.DeleteAsync(labelId);
        }

        public async Task AddLabelToNote(int noteId, int labelId)
        {
            await _noteLabelRepo.AddAsync(new NoteLabel
            {
                NoteId = noteId,
                LabelId = labelId
            });
        }

        public async Task RemoveLabelFromNote(int noteId, int labelId)
        {
            var noteLabels = await _noteLabelRepo.GetAllAsync(nl =>
                nl.NoteId == noteId && nl.LabelId == labelId);
            foreach (var noteLabel in noteLabels)
            {
                await _noteLabelRepo.DeleteAsync(noteLabel);
            }
        }

        public async Task<Label> GetLabelByIdAsync(int labelId)
        {
            return await _labelRepo.GetByIdAsync(labelId);
        }
    }
}