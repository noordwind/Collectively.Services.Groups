using Collectively.Common.Domain;

namespace Collectively.Services.Groups.Domain
{
    public class User : IdentifiableEntity
    {
        public string UserId { get; protected set; }
        public string Name { get; protected set; }
        public string Role { get; protected set; }
        public string State { get; protected set; }
        public string AvatarUrl { get; protected set; }

        protected User()
        {
        }

        public User(string userId, string name, string role, string state, string avatarUrl)
        {
            UserId = userId;
            Name = name;
            Role = role;
            State = state;
            AvatarUrl = avatarUrl;
        }

        public void MarkAsDeleted()
        {
            State = "deleted";
        }
    }
}