using AutoMapper;
using RatingApi.Models;
using RatingApi.Entities;

namespace RatingApi.Profiles
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, RatingDto>();
            CreateMap<RatingForCreationDto, Rating>();
            CreateMap<RatingForUpdateDto, Rating>();
        }
    }
}