using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;

namespace Collectively.Services.Groups.Services
{
    public interface ITagManager
    {
        Task<Maybe<IEnumerable<GroupTag>>> FindAsync(IEnumerable<Guid> tagsIds);
    }
}