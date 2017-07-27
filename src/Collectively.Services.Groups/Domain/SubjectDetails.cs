using System;
using System.Collections.Generic;
using Collectively.Common.Domain;

namespace Collectively.Services.Groups.Domain
{
    public class SubjectDetails : ValueObject<SubjectDetails>
    {
        private ISet<string> _media = new HashSet<string>();
        public string Description { get; protected set; }
        public string Location { get; protected set; }
        public string Website { get; protected set; }
        public string Email { get; protected set; }
        public string Phone { get; protected set; }
        public string PictureUrl { get; protected set; } 
        public IEnumerable<string> Media
        {
            get { return _media; }
            protected set { _media = new HashSet<string>(value); }
        }   

        protected override bool EqualsCore(SubjectDetails other)
        => Website == other.Website || Email == other.Email || Phone == other.Phone;

        protected override int GetHashCodeCore()
        => (Website?.GetHashCode() + Email?.GetHashCode() + Phone?.GetHashCode()) ?? 0;
    }
}