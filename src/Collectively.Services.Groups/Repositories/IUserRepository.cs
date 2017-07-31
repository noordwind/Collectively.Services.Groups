using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;

namespace Collectively.Services.Groups.Repositories
{
    public interface IUserRepository
    {
        Task<Maybe<User>> GetAsync(string userId);
        Task AddAsync(User user);
        Task UpdateAsync(User user);         
    }
}