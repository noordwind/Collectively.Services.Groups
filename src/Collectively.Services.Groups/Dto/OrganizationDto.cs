using System;

namespace Collectively.Services.Groups.Dto
{
    public class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Codename { get; set; }
        public bool IsPublic { get; set; }        
    }
}