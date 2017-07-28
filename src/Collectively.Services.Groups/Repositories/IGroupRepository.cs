using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;

namespace Collectively.Services.Groups.Repositories
{
    public interface IGroupRepository
    {
        Task<bool> ExistsAsync(string name); 
        Task<Maybe<Group>> GetAsync(Guid id);
        Task<Maybe<Group>> GetAsync(string name);
        Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(Guid id);         
    }
}