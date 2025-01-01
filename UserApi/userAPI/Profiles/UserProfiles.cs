using AutoMapper;

namespace userAPI.Profiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles() { 
            CreateMap<Entities.User, DTOs.UserDto>();
            CreateMap<Entities.User, DTOs.UserCreationDto>();
            CreateMap<DTOs.UserCreationDto, Entities.User>();
            CreateMap<DTOs.UserDto, Entities.User>();
        }
    }
}
