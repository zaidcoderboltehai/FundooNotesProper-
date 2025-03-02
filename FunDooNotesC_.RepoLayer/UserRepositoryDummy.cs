using System.Collections.Generic; // List aur collections handle karne ke liye
using System.Linq; // LINQ queries use karne ke liye
using System.Threading.Tasks; // Asynchronous methods ke liye
using FunDooNotesC_.DataLayer.Entities; // User entity ko access karne ke liye namespace include kiya hai

namespace FunDooNotesC_.RepoLayer
{
    // Dummy repository jo bina database ke testing ke liye use hota hai
    public class UserRepositoryDummy : IUserRepository
    {
        // Ek temporary in-memory list jo users ko store karegi
        private readonly List<User> _users = new List<User>();

        // Naya user add karne ke liye asynchronous method
        public Task AddAsync(User entity)
        {
            entity.Id = _users.Count + 1; // User ko ek unique ID assign kar raha hai
            _users.Add(entity); // User ko list me add kar raha hai
            return Task.CompletedTask; // CompletedTask return kar raha hai kyunki koi real database operation nahi hai
        }

        // Saare users ko retrieve karne ke liye method
        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<User>)_users); // Users list ko return kar raha hai
        }

        // ID ke basis pe ek specific user ko find karne ka method
        public Task<User?> GetByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id); // List me se matching ID wala user dhoond raha hai
            return Task.FromResult(user); // User return kar raha hai agar mila toh, nahi toh null
        }

        // User data ko update karne ka method
        public Task UpdateAsync(User entity)
        {
            var user = _users.FirstOrDefault(u => u.Id == entity.Id); // Pehle existing user ko find kar raha hai
            if (user != null) // Agar user mil gaya toh uska data update kar raha hai
            {
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Email = entity.Email;
                user.PasswordHash = entity.PasswordHash;
            }
            return Task.CompletedTask; // Database nahi hai isliye CompletedTask return kar raha hai
        }

        // ID ke basis pe user delete karne ka method
        public Task DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id); // Pehle user ko list me dhoond raha hai
            if (user != null) // Agar user mil gaya toh use remove kar raha hai
            {
                _users.Remove(user);
            }
            return Task.CompletedTask; // Asynchronous method ko satisfy karne ke liye CompletedTask return kar raha hai
        }

        // Email ke basis pe ek user find karne ka method
        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email); // Email match karne wala user dhoond raha hai
            return Task.FromResult(user); // User return kar raha hai agar mila toh, nahi toh null
        }
    }
}
