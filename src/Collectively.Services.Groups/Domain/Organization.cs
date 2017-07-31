using System;
using System.Collections.Generic;
using Collectively.Common.Domain;
using Collectively.Services.Groups.Framework;

namespace Collectively.Services.Groups.Domain
{
    public class Organization : IdentifiableEntity, ITimestampable
    {
        private IDictionary<string,string> _criteria = new Dictionary<string,string>();
        private ISet<Guid> _groups = new HashSet<Guid>();
        private ISet<Member> _members = new HashSet<Member>();
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
        public IDictionary<string,string> Criteria
        {
            get { return _criteria; }
            protected set { _criteria = new Dictionary<string,string>(value); }
        }

        protected Organization()
        {
        } 

        public Organization(string name, Member member, IDictionary<string,string> criteria)
        {
            Name = name;
            Codename = name.ToCodename();
            _members.Add(member);
            _criteria = criteria;            
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }        
    }
}