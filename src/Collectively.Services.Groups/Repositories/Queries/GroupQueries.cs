using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Services.Groups.Domain;
using Collectively.Common.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Collectively.Services.Groups.Queries;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups.Repositories.Queries
{
    public static class GroupQueries
    {
        public static IMongoCollection<Group> Groups(this IMongoDatabase database)
            => database.GetCollection<Group>();

        public static async Task<bool> ExistsAsync(this IMongoCollection<Group> groups, string codename)
            => await groups.AsQueryable().AnyAsync(x => x.Codename == codename);

        public static async Task<Group> GetAsync(this IMongoCollection<Group> groups, Guid id)
        {
            if (id.IsEmpty())
                return null;

            return await groups.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public static async Task<Group> GetAsync(this IMongoCollection<Group> groups, string name)
        {
            if (name.Empty())
                return null;

            var codename = name.ToCodename();

            return await groups.AsQueryable().FirstOrDefaultAsync(x => x.Codename == codename);
        }

        public static IMongoQueryable<Group> Query(this IMongoCollection<Group> groups, 
            BrowseGroups query)
        {
            var values = groups.AsQueryable();

            return values.OrderBy(x => x.Name);
        }
    }
}