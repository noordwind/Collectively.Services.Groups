using System;

namespace Collectively.Services.Groups.Dto
{
    public class MemberDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsActive { get; set; }
    }
}