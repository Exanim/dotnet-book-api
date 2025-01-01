using AutoMapper;

namespace ReviewAPI.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile() 
        {
            CreateMap<Entities.Review, Models.ReviewDto>();
            CreateMap<Models.ReviewForCreationDto, Entities.Review>();
            CreateMap<Models.ReviewForUpdateDto, Entities.Review>();
        }
    }
}
