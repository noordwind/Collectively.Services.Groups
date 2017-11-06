using System;

namespace Collectively.Services.Groups.Dto
{
    public class GroupTagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DefaultId { get; set; }
        public string Default { get; set; }        
    }
}