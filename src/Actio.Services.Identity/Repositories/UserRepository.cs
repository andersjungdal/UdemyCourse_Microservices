using Actio.Services.Identity.Domain.Models;
using Actio.Services.Identity.Domain.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
namespace Actio.Services.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase mongoDatabase;

        public UserRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }
        public async Task<User> GetAsync(Guid id)
            => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);
        public async Task<User> GetAsync(string email)
           => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Email == email.ToLowerInvariant());
        public async Task AddAsync(User user)
            => await Collection.InsertOneAsync(user);


        private IMongoCollection<User> Collection => mongoDatabase.GetCollection<User>("Users");
    }
}
