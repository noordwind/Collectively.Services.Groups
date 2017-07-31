using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Groups.Queries
{
    public class GetOrganization : IQuery
    {
        public Guid Id { get; set; }
    }
}