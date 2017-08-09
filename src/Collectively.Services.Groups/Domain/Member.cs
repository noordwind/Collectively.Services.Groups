namespace Collectively.Services.Groups.Domain
{
    public class Member
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }
        public string Role { get; protected set; }
        public bool IsActive { get; protected set; }

        protected Member()
        {
        }

        protected Member(string role, string userId, string name)
        {
            Role = role;
            UserId = userId;
            Name = name;
            IsActive = true;
        }

        public static Member Participant(string userId, string name)
        => new Member(Roles.Participant, userId, name);

        public static Member Moderator(string userId, string name)
        => new Member(Roles.Moderator, userId, name);

        public static Member Administrator(string userId, string name)
        => new Member(Roles.Administrator, userId, name);

        public static Member Owner(string userId, string name)
        => new Member(Roles.Owner, userId, name);

        public static class Roles 
        {
            public static string Participant => "participant";
            public static string Moderator => "moderator";
            public static string Administrator => "administrator";
            public static string Owner => "owner";
        }        
    }
}