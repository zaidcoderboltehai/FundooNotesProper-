using System; // Basic system functionalities ke liye namespace include kiya hai.
using System.Collections.Generic; // List<T> jaise collections ko use karne ke liye include kiya hai.
using System.Linq; // LINQ queries likhne ke liye include kiya hai.
using System.Linq.Expressions; // Expression<Func<T, bool>> ke liye use hota hai, jo filtering me kaam aata hai.
using System.Threading.Tasks; // Async programming ke liye Task library include kiya hai.
using FunDooNotesC_.DataLayer.Entities; // User entity ko access karne ke liye namespace include kiya hai.

namespace FunDooNotesC_.RepoLayer
{
    // Dummy repository jo bina database ke testing ke liye use hota hai.
    public class UserRepositoryDummy : IUserRepository
    {
        // Ek temporary in-memory list jo users ko store karegi.
        private readonly List<User> _users = new List<User>();

        // Naya user add karne ke liye asynchronous method.
        public Task AddAsync(User entity)
        {
            entity.Id = _users.Count + 1; // User ko ek unique ID assign kar raha hai (List ki length + 1).
            _users.Add(entity); // User ko list me add kar raha hai.
            return Task.CompletedTask; // CompletedTask return kar raha hai kyunki koi real database operation nahi hai.
        }

        // Saare users ko retrieve karne ke liye method.
        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<User>)_users); // Users list ko return kar raha hai.
        }

        // Naya overload: Predicate ke basis par filtered users ko fetch karne ke liye method.
        public Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
        {
            // _users ko AsQueryable() bana ke predicate apply kar rahe hain, phir result ko list me convert kar rahe hain.
            var filteredUsers = _users.AsQueryable().Where(predicate).AsEnumerable();
            return Task.FromResult(filteredUsers); // Filtered users return ho rahe hain.
        }

        // ID ke basis pe ek specific user ko find karne ka method.
        public Task<User?> GetByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id); // List me se matching ID wala user dhoond raha hai.
            return Task.FromResult(user); // User return kar raha hai agar mila toh, nahi toh null.
        }

        // User data ko update karne ka method.
        public Task UpdateAsync(User entity)
        {
            var user = _users.FirstOrDefault(u => u.Id == entity.Id); // Pehle, existing user ko find kar raha hai.
            if (user != null) // Agar user mil gaya toh uska data update kar raha hai.
            {
                user.FirstName = entity.FirstName; // First Name update kar raha hai.
                user.LastName = entity.LastName; // Last Name update kar raha hai.
                user.Email = entity.Email; // Email update kar raha hai.
                user.PasswordHash = entity.PasswordHash; // Password update kar raha hai.
            }
            return Task.CompletedTask; // Database nahi hai isliye CompletedTask return kar raha hai.
        }

        // ID ke basis pe user delete karne ka method.
        public Task DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id); // Pehle user ko list me dhoond raha hai.
            if (user != null) // Agar user mil gaya toh use remove kar raha hai.
            {
                _users.Remove(user); // User ko list se remove kar diya.
            }
            return Task.CompletedTask; // Asynchronous method ko satisfy karne ke liye CompletedTask return kar raha hai.
        }

        // Email ke basis pe ek user find karne ka method.
        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email); // Email match karne wala user dhoond raha hai.
            return Task.FromResult(user); // User return kar raha hai agar mila toh, nahi toh null.
        }
    }
}
