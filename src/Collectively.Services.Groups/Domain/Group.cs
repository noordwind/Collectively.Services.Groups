using System;
using System.Collections.Generic;
using System.Linq;
using Collectively.Common.Domain;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups.Domain
{
    public class Group : IdentifiableEntity, ITimestampable
    {
        private IDictionary<string,string> _criteria = new Dictionary<string,string>();
        private ISet<Member> _members = new HashSet<Member>();
        private ISet<string> _locations = new HashSet<string>();
        public Guid? OrganizationId { get; protected set; }
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
        public IEnumerable<string> Locations
        {
            get { return _locations; }
            protected set { _locations = new HashSet<string>(value); }
        }
        public IDictionary<string,string> Criteria
        {
            get { return _criteria; }
            protected set { _criteria = new Dictionary<string,string>(value); }
        }

        protected Group()
        {
        } 

        public Group(Guid id, string name, Member member, bool isPublic,
            IDictionary<string,string> criteria, Guid? organizationId = null,
            IEnumerable<string> locations = null)
        {
            if(name.Length > 100)
            {
                throw new DomainException(OperationCodes.InvalidName, "Invalid group name.");
            }
            Id = id;
            Name = name;
            State = "active";
            Codename = name.ToCodename();
            _members.Add(member);
            IsPublic = isPublic;
            OrganizationId = organizationId;
            _criteria = criteria ?? new Dictionary<string,string>()
            {
                ["create_remark"] = Domain.Criteria.CreateRemark.Public
            }; 
            _locations = locations == null ? new HashSet<string>() : new HashSet<string>(locations);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }  
    }
}