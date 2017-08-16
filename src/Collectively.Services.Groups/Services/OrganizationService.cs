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
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;

        public OrganizationService(IOrganizationRepository organizationRepository,
            IUserRepository userRepository)
        {
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _organizationRepository.ExistsAsync(name);

        public async Task<Maybe<Organization>> GetAsync(Guid id)
        => await _organizationRepository.GetAsync(id);

        public async Task<Maybe<PagedResult<Organization>>> BrowseAsync(BrowseOrganizations query)
        => await _organizationRepository.BrowseAsync(query);

        public async Task CreateAsync(Guid id, string name, string userId, 
            bool isPublic, IDictionary<string,ISet<string>> criteria)
        {
            if(await ExistsAsync(name))
            {
                throw new ServiceException(OperationCodes.OrganizationNameInUse,
                    $"Organization with name: '{name}' already exists.");
            }
            var user = await _userRepository.GetAsync(userId);
            var owner = Member.Owner(user.Value.UserId, user.Value.Name);
            var organization = new Organization(id, name, owner, isPublic, criteria);
            await _organizationRepository.AddAsync(organization);
        }

        public async Task AddMemberAsync(Guid id, string memberId, string role)
        {
            var organization = await GetAsync(id);
            if(organization.HasNoValue)
            {
                throw new ServiceException(OperationCodes.OrganizationNotFound,
                    $"Organization with id: '{id}' was not found.");
            }
            var user = await _userRepository.GetAsync(memberId);
            if(user.HasNoValue)
            {
                throw new ServiceException(OperationCodes.UserNotFound,
                    $"User with id: '{memberId}' was not found.");
            }
            organization.Value.AddMember(Member.FromRole(user.Value.UserId, user.Value.Name, role));
            await _organizationRepository.UpdateAsync(organization.Value);
        }
    }
}