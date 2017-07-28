using System;
using System.Threading.Tasks;
using Collectively.Common.Mongo;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;
using Collectively.Services.Groups.Repositories.Queries;
using MongoDB.Driver;

namespace Collectively.Services.Groups.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IMongoDatabase _database;

        public OrganizationRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _database.Organizations().ExistsAsync(name);

        public async Task<Maybe<Organization>> GetByIdAsync(Guid id)
        => await _database.Organizations().GetByIdAsync(id);

        public async Task<Maybe<Organization>> GetByCodenameAsync(string codename)
        => await _database.Organizations().GetByCodenameAsync(codename);

        public async Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query)
        => await _database.Organizations().Query(query).PaginateAsync(query);

        public async Task AddAsync(Organization organization)
        => await _database.Organizations().InsertOneAsync(organization);

        public async Task UpdateAsync(Organization organization)
        => await _database.Organizations().ReplaceOneAsync(x => x.Id == organization.Id, organization);

        public async Task DeleteAsync(Guid id)
        => await _database.Organizations().DeleteOneAsync(x => x.Id == id);
    }
}