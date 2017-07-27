using System;
using System.Collections.Generic;
using Collectively.Common.Domain;

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
    }
}