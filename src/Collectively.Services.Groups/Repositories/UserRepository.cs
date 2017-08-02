using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Repositories.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Groups.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<User>> GetAsync(string userId)
            => await _database.Users().GetAsync(userId);

        public async Task AddAsync(User user)
            => await _database.Users().InsertOneAsync(user);

        public async Task UpdateAsync(User user)
            => await _database.Users().ReplaceOneAsync(x => x.UserId == user.UserId, user);

        public async Task DeleteAsync(string userId)
            => await _database.Users().DeleteOneAsync(x => x.UserId == userId);
    }
}