// FunDooNotesC_.RepoLayer/LabelRepository.cs
using FunDooNotesC_.DataLayer.Entities;
using FunDooNotesC_.DataLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace FunDooNotesC_.RepoLayer
{
    public class LabelRepository : Repository<Label>, ILabelRepository
    {
        public LabelRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Label>> GetLabelsByUser(int userId)
        {
            return await _context.Labels
                .Where(l => l.UserId == userId)
                .ToListAsync();
        }

        // Implement other IRepository methods if needed (they are inherited from Repository<Label>)
    }
}