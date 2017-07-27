using System;

namespace Collectively.Services.Groups.Domain
{
    public class OrganizationInfo
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Codename { get; protected set; }
    }
}