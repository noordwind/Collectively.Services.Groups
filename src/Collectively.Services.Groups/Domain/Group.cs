using System;
using System.Collections.Generic;
using Collectively.Common.Domain;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups.Domain
{
    public class Group : IdentifiableEntity, ITimestampable
    {
        private IDictionary<string,string> _criteria = new Dictionary<string,string>();
        private ISet<Member> _members = new HashSet<Member>();
        public OrganizationInfo Organization { get; protected set; }
        public string Name { get; protected set; }
        public string Codename { get; protected set; }
        public bool IsPublic { get; protected set; }
        public SubjectDetails Details { get; protected set; }
        public string State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public IEnumerable<Member> Members
        {
            get { return _members; }
            protected set { _members = new HashSet<Member>(value); }
        }
        public IDictionary<string,string> Criteria
        {
            get { return _criteria; }
            protected set { _criteria = new Dictionary<string,string>(value); }
        }

        protected Group()
        {
        } 

        public Group(string name, Member member, bool isPublic,
            IDictionary<string,string> criteria, Organization organization = null)
        {
            if(name.Length > 100)
            {
                throw new DomainException(OperationCodes.InvalidName, "Invalid group name.");
            }
            Name = name;
            Codename = name.ToCodename();
            _members.Add(member);
            IsPublic = isPublic;
            _criteria = criteria ?? new Dictionary<string,string>(); 
            Organization = organization == null ? null : OrganizationInfo.Create(organization);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }  
    }
}