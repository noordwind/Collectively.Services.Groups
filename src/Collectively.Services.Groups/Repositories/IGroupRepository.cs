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
        Task<Maybe<Group>> GetByIdAsync(Guid id);
        Task<Maybe<Group>> GetByCodenameAsync(string codename);
        Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(Guid id);         
    }
}