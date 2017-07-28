using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;

namespace Collectively.Services.Groups.Repositories
{
    public interface IOrganizationRepository
    {
        Task<bool> ExistsAsync(string name); 
        Task<Maybe<Organization>> GetAsync(Guid id);
        Task<Maybe<Organization>> GetAsync(string name);
        Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query);
        Task AddAsync(Organization organization);
        Task UpdateAsync(Organization organization);
        Task DeleteAsync(Guid id);         
    }
}