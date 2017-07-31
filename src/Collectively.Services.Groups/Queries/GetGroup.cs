using System;
using Collectively.Common.Queries;

namespace Collectively.Services.Groups.Queries
{
    public class GetGroup : IQuery
    {
        public Guid Id { get; set; }
    }
}