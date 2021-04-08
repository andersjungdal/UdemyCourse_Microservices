using Actio.Api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Actio.Api.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly IMongoDatabase database;

        public ActivityRepository(IMongoDatabase database)
        {
            this.database = database;
        }
        public async Task AddAsync(Activity model) => await Collection.InsertOneAsync(model);
        public async Task<IEnumerable<Activity>> BrowseAsync(Guid userId, string category)
            => await Collection
            .AsQueryable()
            .Where(x => x.UserId == userId)
            .ToListAsync();
        public async Task<Activity> GetAsync(Guid id)
            => await Collection
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);

        public Task<IEnumerable<Activity>> BrowseAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        private IMongoCollection<Activity> Collection
            => database.GetCollection<Activity>("Activities");
    }
}
