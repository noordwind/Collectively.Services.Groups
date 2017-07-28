using System;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;

namespace Collectively.Services.Groups.Services
{
    public interface IGroupService
    {
        Task<bool> ExistsAsync(string name);
        Task<Maybe<Group>> GetAsync(Guid id);
        Task CreateAsync(string name, string userId);
    }
}