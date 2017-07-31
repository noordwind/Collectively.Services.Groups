using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;

namespace Collectively.Services.Groups.Services
{
    public interface IGroupService
    {
        Task<bool> ExistsAsync(string name);
        Task<Maybe<Group>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query);
        Task CreateAsync(string name, string userId, 
            IDictionary<string,string> criteria, Guid? organizationId = null);
    }
}