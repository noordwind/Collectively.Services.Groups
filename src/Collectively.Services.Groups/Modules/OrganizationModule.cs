using AutoMapper;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Dto;
using Collectively.Services.Groups.Queries;
using Collectively.Services.Groups.Services;

namespace Collectively.Services.Groups.Modules
{
    public class OrganizationModule : ModuleBase
    {
        public OrganizationModule(IOrganizationService organizationService, IMapper mapper) : base(mapper, "organizations")
        {
            Get("", async args => await FetchCollection<BrowseOrganizations, Organization>
                (async x => await organizationService.BrowseAsync(x))
                .MapTo<OrganizationDto>()
                .HandleAsync());

            Get("{id}", async args => await Fetch<GetOrganization, Organization>
                (async x => await organizationService.GetAsync(x.Id))
                .MapTo<OrganizationDto>()
                .HandleAsync());
        }        
    }
}