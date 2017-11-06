using System;
using Collectively.Common.Domain;
using Collectively.Common.Extensions;

namespace Collectively.Services.Groups.Domain
{
    public class GroupTag : ValueObject<GroupTag>
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public Guid DefaultId { get; set; }
        public string Default { get; protected set; }

        protected GroupTag() 
        {
        }

        protected GroupTag(Guid id, string name, Guid defaultId, string @default)
        {
            if (name.Empty())
            {
                throw new ArgumentException("Tag name can not be empty.", nameof(name));
            }
            if (@default.Empty())
            {
                throw new ArgumentException("Tag default name can not be empty.", nameof(name));
            }
            Id = id;
            Name = name;
            DefaultId = defaultId;
            Default = @default;
        }

        public static GroupTag Create(Guid id, string name, Guid defaultId, string @default)
            => new GroupTag(id, name, defaultId, @default);

        protected override bool EqualsCore(GroupTag other) 
            => Id.Equals(other.Id);

        protected override int GetHashCodeCore() 
            => Id.GetHashCode();
    }
}