using AutoMapper;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Dto;
using Collectively.Services.Groups.Queries;
using Collectively.Services.Groups.Services;

namespace Collectively.Services.Groups.Modules
{
    public class GroupModule : ModuleBase
    {
        public GroupModule(IGroupService groupService, IMapper mapper) : base(mapper, "groups")
        {
            Get("", async args => await FetchCollection<BrowseGroups, Group>
                (async x => await groupService.BrowseAsync(x))
                .MapTo<GroupDto>()
                .HandleAsync());

            Get("{id}", async args => await Fetch<GetGroup, Group>
                (async x => await groupService.GetAsync(x.Id))
                .MapTo<GroupDto>()
                .HandleAsync());
        }        
    }
}