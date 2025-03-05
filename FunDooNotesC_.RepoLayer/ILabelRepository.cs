// FunDooNotesC_.RepoLayer/ILabelRepository.cs
using FunDooNotesC_.DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunDooNotesC_.RepoLayer
{
    public interface ILabelRepository : IRepository<Label>
    {
        Task<IEnumerable<Label>> GetLabelsByUser(int userId);
    }
}