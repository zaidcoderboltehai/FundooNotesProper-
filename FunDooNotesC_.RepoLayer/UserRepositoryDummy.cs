using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FunDooNotesC_.DataLayer.Entities;

namespace FunDooNotesC_.RepoLayer
{
    public class UserRepositoryDummy : IUserRepository
    {
        private readonly List<User> _users = new List<User>();

        public Task AddAsync(User entity)
        {
            entity.Id = _users.Count + 1;
            _users.Add(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult((IEnumerable<User>)_users);
        }

        public Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
        {
            var filteredUsers = _users.AsQueryable().Where(predicate).AsEnumerable();
            return Task.FromResult(filteredUsers);
        }

        public Task<User?> GetByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task UpdateAsync(User entity)
        {
            var user = _users.FirstOrDefault(u => u.Id == entity.Id);
            if (user != null)
            {
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Email = entity.Email;
                user.PasswordHash = entity.PasswordHash;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null) _users.Remove(user);
            return Task.CompletedTask;
        }

        // Add this missing method
        public Task DeleteAsync(User entity)
        {
            _users.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }
    }
}