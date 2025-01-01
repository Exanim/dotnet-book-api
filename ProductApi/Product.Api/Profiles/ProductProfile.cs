using AutoMapper;
using Products.Api.Models;

namespace Products.Api.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Entities.Product, Models.ProductDto>();
            CreateMap<Entities.Product,Models.ProductWithIdDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductDto() {ProductName = src.ProductName } ));
            CreateMap<Models.ProductDto,Entities.Product>();
        }
    }
}
