using Microsoft.EntityFrameworkCore; // Entity Framework Core ka use kar rahe hain, jo ORM (Object-Relational Mapping) hai.
using FunDooNotesC_.DataLayer.Entities; // User entity ko access karne ke liye namespace include kiya hai.
using System.Threading.Tasks; // Asynchronous programming ke liye Task library include kiya hai.
using FunDooNotesC_.DataLayer; // Database context ko access karne ke liye namespace include kiya hai.

namespace FunDooNotesC_.RepoLayer
{
    // UserRepository class jo Repository<User> ko inherit karti hai aur IUserRepository ko implement karti hai.
    public class UserRepository : Repository<User>, IUserRepository
    {
        // Constructor jo ApplicationDbContext ka instance accept karta hai aur base Repository class ko initialize karta hai.
        public UserRepository(ApplicationDbContext context)
            : base(context) // Base class (Repository<User>) ka constructor call ho raha hai.
        { }

        // Email ke basis pe ek specific user ko fetch karne ke liye asynchronous method.
        public async Task<User?> GetByEmailAsync(string email)
        {
            // Database ke Users table se pehla matching record fetch karega jiska email match karega.
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
