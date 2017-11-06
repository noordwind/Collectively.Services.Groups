using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;

namespace Collectively.Services.Groups.Repositories
{
    public interface ITagRepository
    {
         Task<Maybe<Tag>> GetAsync(string name);
         Task<Maybe<PagedResult<Tag>>> BrowseAsync(BrowseTags query);
         Task AddAsync(Tag tag);
         Task AddAsync(IEnumerable<Tag> tags);
    }
}