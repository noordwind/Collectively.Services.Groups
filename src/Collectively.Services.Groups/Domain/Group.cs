using System;
using System.Collections.Generic;
using System.Linq;
using Collectively.Common.Domain;
using Collectively.Common.Extensions;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups.Domain
{
    public class Group : IdentifiableEntity, ITimestampable
    {
        private IDictionary<string,ISet<string>> _criteria = new Dictionary<string,ISet<string>>();
        private ISet<Member> _members = new HashSet<Member>();
        private ISet<GroupTag> _tags = new HashSet<GroupTag>();
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
        public IDictionary<string,ISet<string>> Criteria
        {
            get { return _criteria; }
            protected set { _criteria = new Dictionary<string,ISet<string>>(value); }
        }
        public IEnumerable<GroupTag> Tags
        {
            get { return _tags; }
            protected set { _tags = new HashSet<GroupTag>(value); }
        }

        protected Group()
        {
        } 

        public Group(Guid id, string name, Member member, bool isPublic,
            IDictionary<string,ISet<string>> criteria,
            IEnumerable<GroupTag> tags, Guid? organizationId = null)
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
            criteria = criteria ?? new Dictionary<string,ISet<string>>();
            _criteria = Domain.Criteria.MergeForGroupOrFail(criteria);
            Tags = tags ?? Enumerable.Empty<GroupTag>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddMember(Member member)
        {
            if(Members.Contains(member))
            {
                throw new DomainException(OperationCodes.GroupMemberAlreadyExists, 
                    $"Group with id: '{Id}' already contains a member: '{member.UserId}'.");
            }
            _members.Add(member);
            UpdatedAt = DateTime.UtcNow;
        }  

        public void AddTag(GroupTag tag)
        {
            _tags.Add(tag);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveTag(Guid id)
        {
            _tags.Remove(_tags.SingleOrDefault(x => x.Id == id));
            UpdatedAt = DateTime.UtcNow;
        }
    }
}