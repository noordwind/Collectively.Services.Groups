using System;
using System.Threading.Tasks;
using Collectively.Common.Domain;
using Collectively.Common.Types;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Repositories;

namespace Collectively.Services.Groups.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<bool> ExistsAsync(string name)
        => await _organizationRepository.ExistsAsync(name);

        public async Task<Maybe<Organization>> GetAsync(Guid id)
        => await _organizationRepository.GetAsync(id);

        public async Task CreateAsync(string name, string userId)
        {
            if(await ExistsAsync(name))
            {
                throw new ServiceException(OperationCodes.OrganizationNameInUse,
                    $"Organization with name: '{name}' already exists.");
            }
            var organization = new Organization(name, userId);
            await _organizationRepository.AddAsync(organization);
        }
    }
}