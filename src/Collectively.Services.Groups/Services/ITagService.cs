using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Services.Groups.Dto;

namespace Collectively.Services.Groups.Services
{
    public interface ITagService
    {
        Task AddOrUpdateAsync(IEnumerable<TagDto> tags);
    }
}