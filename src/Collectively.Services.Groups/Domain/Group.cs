using System;
using System.Collections.Generic;
using System.Linq;
using Collectively.Common.Domain;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups.Domain
{
    public class Group : IdentifiableEntity, ITimestampable
    {
        private IDictionary<string,ISet<string>> _criteria = new Dictionary<string,ISet<string>>();
        private ISet<Member> _members = new HashSet<Member>();
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

        protected Group()
        {
        } 

        public Group(Guid id, string name, Member member, bool isPublic,
            IDictionary<string,ISet<string>> criteria, Guid? organizationId = null)
        {
            if(name.Length > 100)
            {
                throw new DomainException(OperationCodes.InvalidName, "Invalid group name.");
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
            OrganizationId = organizationId;
            _criteria = new Dictionary<string,ISet<string>>()
            {
                ["create_remark"] = new HashSet<string>{Domain.Criteria.CreateRemark.Public},
                ["resolve_remark"] = new HashSet<string>{Domain.Criteria.ResolveRemark.Public},
                ["membership"] = new HashSet<string>{Domain.Criteria.Membership.Invitation}
            };
            foreach(var rule in criteria)
            {
                _criteria[rule.Key.ToLowerInvariant()] = rule.Value;
            } 
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }  
    }
}