using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;

namespace Collectively.Services.Groups.Services
{
    public interface IOrganizationService
    {
        Task<bool> ExistsAsync(string name);
        Task<Maybe<Organization>> GetAsync(Guid id);
        Task CreateAsync(string name, string userId, IDictionary<string,string> criteria);
    }
}