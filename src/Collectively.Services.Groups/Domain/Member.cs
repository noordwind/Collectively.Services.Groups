using Collectively.Common.Domain;

namespace Collectively.Services.Groups.Domain
{
    public class Member : ValueObject<Member>
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

        protected override bool EqualsCore(Member other)
        => UserId == other.UserId;

        protected override int GetHashCodeCore()
        => UserId.GetHashCode();

        public static Member Participant(string userId, string name)
        => new Member(Roles.Participant, userId, name);

        public static Member Moderator(string userId, string name)
        => new Member(Roles.Moderator, userId, name);

        public static Member Administrator(string userId, string name)
        => new Member(Roles.Administrator, userId, name);

        public static Member Owner(string userId, string name)
        => new Member(Roles.Owner, userId, name);

        public static Member FromRole(string userId, string name, string role)
        {
            switch(role.ToLowerInvariant())
            {
                case "participant": return Participant(userId, name);
                case "moderator": return Moderator(userId, name);
                case "administrator": return Administrator(userId, name);
                case "owner": return Owner(userId, name);
            }
            throw new DomainException(OperationCodes.InvalidMemberRole, 
                $"Invalid member role: '{role}' for user with id: '{userId}'.");
        }

        public static class Roles 
        {
            public static string Participant => "participant";
            public static string Moderator => "moderator";
            public static string Administrator => "administrator";
            public static string Owner => "owner";
        }        
    }
}