namespace Collectively.Services.Groups.Domain
{
    public class Member
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }
        public string Role { get; protected set; }
        public string State { get; protected set; }
        public string AvatarUrl { get; protected set; }

        public static class Roles 
        {
            public static string User => "user";
            public static string Moderator => "moderator";
            public static string Administrator => "administrator";
        }        
    }
}