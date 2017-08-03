using System;
using System.Collections.Generic;
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
            criteria = criteria ?? new Dictionary<string,ISet<string>>();
            if(criteria.Count > 100)
            {
                throw new DomainException(OperationCodes.TooManyCriteria, 
                    $"Too many criteria: {criteria.Count} (max: 100).");
            }
            Id = id;
            Name = name;
            State = "active";
            Codename = name.ToCodename();
            _members.Add(member);
            IsPublic = isPublic;
            _criteria = new Dictionary<string,ISet<string>>()
            {
                ["membership"] = new HashSet<string>{Domain.Criteria.Membership.Invitation}
            };
            foreach(var rule in criteria)
            {
                _criteria[rule.Key.ToLowerInvariant()] = rule.Value;
            }           
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddGroup(Group group)
        {
            _groups.Add(group.Id);
        }        
    }
}