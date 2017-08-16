using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;

namespace Collectively.Services.Groups.Services
{
    public interface IOrganizationService
    {
        Task<bool> ExistsAsync(string name);
        Task<Maybe<Organization>> GetAsync(Guid id);
        Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query);
        Task CreateAsync(Guid id, string name, string userId, 
            bool isPublic, IDictionary<string,ISet<string>> criteria);
        Task AddMemberAsync(Guid id, string memberId, string role);
    }
}