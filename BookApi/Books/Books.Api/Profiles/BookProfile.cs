using AutoMapper;

namespace Books.Api.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile() 
        {
            CreateMap<Entity.BookEntity, Org.OpenAPITools.Models.Book>();
            CreateMap<Models.BookForCreation, Entity.BookEntity>();
            CreateMap<Models.BookForUpdate, Entity.BookEntity>();
        }
    }
}
