using System;
using System.Collections.Generic;
using System.Linq;
using Collectively.Common.Domain;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups.Domain
{
    public class Organization : IdentifiableEntity, ITimestampable
    {
        private IDictionary<string,ISet<string>> _criteria = new Dictionary<string,ISet<string>>();
        private ISet<Member> _members = new HashSet<Member>();
        private ISet<Guid> _groups = new HashSet<Guid>();
        public string Name { get; protected set; }
        public string Codename { get; protected set; }
        public bool IsPublic { get; protected set; }
        public SubjectDetails Details { get; protected set; }
        public string State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public IEnumerable<Guid> Groups
        {
            get { return _groups; }
            protected set { _groups = new HashSet<Guid>(value); }
        }    
        public IEnumerable<Member> Members
        {
            get { return _members; }
            protected set { _members = new HashSet<Member>(value); }
        }
        public IDictionary<string,ISet<string>> Criteria
        {
            get { return _criteria; }
            protected set { _criteria = new Dictionary<string,ISet<string>>(value); }
        }

        protected Organization()
        {
        } 

        public Organization(Guid id, string name, Member member, 
            bool isPublic, IDictionary<string,ISet<string>> criteria)
        {
            if(name.Length > 100)
            {
                throw new DomainException(OperationCodes.InvalidName, "Invalid organization name.");
            }
            Id = id;
            Name = name;
            State = "active";
            Codename = name.ToCodename();
            _members.Add(member);
            IsPublic = isPublic;
            criteria = criteria ?? new Dictionary<string,ISet<string>>();
            _criteria = Domain.Criteria.MergeForOrganizationOrFail(criteria);          
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddGroup(Group group)
        {
            _groups.Add(group.Id);
        }

        public void AddMember(Member member)
        {
            if(Members.Contains(member))
            {
                throw new DomainException(OperationCodes.OrganizationMemberAlreadyExists, 
                    $"Organization with id: '{Id}' already contains a member: '{member.UserId}'.");
            }
            _members.Add(member);
            UpdatedAt = DateTime.UtcNow;
        }  
    }
}