using System;

namespace Collectively.Services.Groups.Domain
{
    public class OrganizationInfo
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Codename { get; protected set; }
        public bool IsPublic { get; protected set; }

        protected OrganizationInfo()
        {
        }

        protected OrganizationInfo(Guid id, string name, string codename, bool isPublic)
        {
            Id = id;
            Name = name;
            Codename = codename;
            IsPublic = isPublic;
        }

        public static OrganizationInfo Create(Organization organization)
        => new OrganizationInfo(organization.Id, organization.Name, 
            organization.Codename, organization.IsPublic);
    }
}