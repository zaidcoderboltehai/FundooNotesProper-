using FunDooNotesC_.DataLayer;
using FunDooNotesC_.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FunDooNotesC_.RepoLayer
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly StackExchange.Redis.IDatabase _redis;

        public UserRepository(ApplicationDbContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis.GetDatabase();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var cacheKey = $"user:{id}";
            var cachedUser = await _redis.StringGetAsync(cacheKey);

            if (!cachedUser.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<User>(cachedUser);
            }

            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                await _redis.StringSetAsync(cacheKey, JsonConvert.SerializeObject(user), TimeSpan.FromMinutes(5));
            }

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var cacheKey = $"user:{user.Id}";
            await _redis.KeyDeleteAsync(cacheKey);
        }
    }
}