using System;
using System.Collections.Generic;

namespace Collectively.Services.Groups.Dto
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Codename { get; set; }
        public bool IsPublic { get; set; } 
        public string State { get; set; }  
        public SubjectDetailsDto Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IList<MemberDto> Members { get; set; }
        public IList<string> Locations { get; set; }
        public IDictionary<string,string> Criteria { get; set; }
    }
}