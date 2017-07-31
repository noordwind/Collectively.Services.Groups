using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Collectively.Common.Domain;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Queries;
using Collectively.Services.Groups.Repositories;

namespace Collectively.Services.Groups.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public GroupService(IGroupRepository groupRepository,
            IUserRepository userRepository, IOrganizationRepository organizationRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _groupRepository.ExistsAsync(name);

        public async Task<Maybe<Group>> GetAsync(Guid id)
        => await _groupRepository.GetAsync(id);

        public async Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query)
        => await _groupRepository.BrowseAsync(query);

        public async Task CreateAsync(string name, string userId, 
            IDictionary<string,string> criteria, Guid? organizationId = null)
        {
            if(await ExistsAsync(name))
            {
                throw new ServiceException(OperationCodes.GroupNameInUse,
                    $"Group with name: '{name}' already exists.");
            }
            var user = await _userRepository.GetAsync(userId);
            var organization = await _organizationRepository.GetAsync(organizationId.Value);
            var owner = Member.Owner(user.Value.UserId, user.Value.Role, user.Value.AvatarUrl);
            var group = new Group(name, owner, criteria, organization.Value);
            await _groupRepository.AddAsync(group);
        }
    }
}