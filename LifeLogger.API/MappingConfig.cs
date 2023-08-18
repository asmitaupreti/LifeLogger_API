using AutoMapper;
using LifeLogger.Models.DTO;
using LifeLogger.Models;

namespace LifeLogger.API
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, ApplicationUserCreateDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserUpdateDTO>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserResponseDTO>().ReverseMap();

            CreateMap<LifeProject, LifeProjectCreateDTO>().ReverseMap();
            CreateMap<LifeProject, LifeProjectUpdateDTO>().ReverseMap();
            CreateMap<LifeProject, LifeProjectResponseDTO>().ReverseMap();

            CreateMap<LifeMilestone, LifeMilestoneCreateDTO>().ReverseMap();
            CreateMap<LifeProject, LifeMilestoneUpdateDTO>().ReverseMap();
            CreateMap<LifeProject, LifeMilestoneResponseDTO>().ReverseMap();

            CreateMap<Tag, TagCreateDTO>().ReverseMap();
            CreateMap<Tag, TagUpdateDTO>().ReverseMap();
        }
    }
}