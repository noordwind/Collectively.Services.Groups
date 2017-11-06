using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ITagRepository _tagRepository;
        private readonly ITagManager _tagManager;

        public GroupService(IGroupRepository groupRepository,
            IUserRepository userRepository, 
            IOrganizationRepository organizationRepository,
            ITagRepository tagRepository,
            ITagManager tagManager)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _tagRepository = tagRepository;
            _tagManager = tagManager;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _groupRepository.ExistsAsync(name);

        public async Task<Maybe<Group>> GetAsync(Guid id)
        => await _groupRepository.GetAsync(id);

        public async Task<Maybe<PagedResult<Group>>> BrowseAsync(BrowseGroups query)
        => await _groupRepository.BrowseAsync(query);

        public async Task CreateAsync(Guid id, string name, string userId, bool isPublic, 
            IDictionary<string,ISet<string>> criteria, IEnumerable<Guid> tags,
            Guid? organizationId = null)
        {

            if(await ExistsAsync(name))
            {
                throw new ServiceException(OperationCodes.GroupNameInUse,
                    $"Group with name: '{name}' already exists.");
            }
            var groupTags = await _tagManager.FindAsync(tags);
            if (groupTags.HasNoValue || !groupTags.Value.Any())
            {
                throw new ServiceException(OperationCodes.TagsNotProvided,
                    $"Tags were not provided for group: '{id}'.");
            }
            var user = await _userRepository.GetAsync(userId);
            var organization = await ValidateIfGroupCanBeAddedToOrganizationAsync(
                id, organizationId, user.Value);
            var owner = Member.Owner(user.Value.UserId, user.Value.Name);
            var group = new Group(id, name, owner, isPublic, criteria, groupTags.Value, organizationId);
            await _groupRepository.AddAsync(group);
            if(organization.HasNoValue)
            {
                return;
            }
            organization.Value.AddGroup(group);
            await _organizationRepository.UpdateAsync(organization.Value);
        }

        private async Task<Maybe<Organization>> ValidateIfGroupCanBeAddedToOrganizationAsync(
            Guid groupId, Guid? organizationId, User user)
        {
            if(!organizationId.HasValue || organizationId.Value == Guid.Empty)
            {
                return null;
            }
            var organization = await _organizationRepository.GetAsync(organizationId.Value);
            if(organization.HasNoValue)
            {
                throw new ServiceException(OperationCodes.OrganizationNotFound, 
                    $"Can not create a new group with id: '{groupId}' - " +
                    $"organization was not found for given id: '{organizationId}'.");
            }
            if(user.Role == "moderator" || user.Role == "administrator")
            {
                return organization;
            }
            var member = organization.Value.Members.SingleOrDefault(x => x.UserId == user.UserId);
            if(member == null)
            {
                throw new ServiceException(OperationCodes.OrganizationMemberNotFound,
                    $"Organization: '{organizationId}' member: '{user.UserId}' was not found.");
            }
            if(member.Role == "administrator" || member.Role == "owner")
            {
                return organization;
            }
                throw new ServiceException(OperationCodes.OrganizationMemberHasInsufficientRole,
                    $"Organization: '{organizationId}' member: '{user.UserId}' ." +
                     "does not have privileges to add a group.");             
        }

        public async Task AddMemberAsync(Guid id, string memberId, string role)
        {
            var group = await GetAsync(id);
            if(group.HasNoValue)
            {
                throw new ServiceException(OperationCodes.GroupNotFound,
                    $"Group with id: '{id}' was not found.");
            }
            var user = await _userRepository.GetAsync(memberId);
            if(user.HasNoValue)
            {
                throw new ServiceException(OperationCodes.UserNotFound,
                    $"User with id: '{memberId}' was not found.");
            }
            group.Value.AddMember(Member.FromRole(user.Value.UserId, user.Value.Name, role));
            await _groupRepository.UpdateAsync(group.Value);
        }
    }
}