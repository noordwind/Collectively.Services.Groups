using AutoMapper;
using Collectively.Services.Groups.Domain;
using Collectively.Services.Groups.Dto;

namespace Collectively.Services.Groups.Framework
{
    public static class AutoMapperConfig
    {
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Group, GroupDto>();
                cfg.CreateMap<GroupTag, GroupTagDto>();
                cfg.CreateMap<Member, MemberDto>();
                cfg.CreateMap<Organization, OrganizationDto>();
                cfg.CreateMap<SubjectDetails, SubjectDetailsDto>();
                cfg.CreateMap<Tag, TagDto>();
                cfg.CreateMap<TranslatedTag, TranslatedTagDto>();
                cfg.CreateMap<User, UserDto>();
            });

            return config.CreateMapper();
        }        
    }
}