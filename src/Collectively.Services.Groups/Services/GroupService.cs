using System;
using System.Threading.Tasks;
using Collectively.Common.Domain;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Repositories;

namespace Collectively.Services.Groups.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _groupRepository.ExistsAsync(name);

        public async Task<Maybe<Group>> GetAsync(Guid id)
        => await _groupRepository.GetAsync(id);

        public async Task CreateAsync(string name, string userId)
        {
            if(await ExistsAsync(name))
            {
                throw new ServiceException(OperationCodes.GroupNameInUse,
                    $"Group with name: '{name}' already exists.");
            }
            var group = new Group(name, userId);
            await _groupRepository.AddAsync(group);
        }
    }
}