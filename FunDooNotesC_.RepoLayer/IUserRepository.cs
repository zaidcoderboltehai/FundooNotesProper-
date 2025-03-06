using FunDooNotesC_.DataLayer.Entities;
using System.Threading.Tasks;

namespace FunDooNotesC_.RepoLayer
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user); // Naya method add karo

    }
}