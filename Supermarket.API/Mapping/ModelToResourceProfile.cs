using AutoMapper;
using Supermarket.API.Domain.Models;
using Supermarket.API.Resources;
using Supermarket.API.Extensions;

namespace Supermarket.API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<Product, ProductDto>()
                .ForMember(src => src.UnitOfMeasurement, 
                opt => opt.MapFrom(src => src.UnitOfMeasurement.ToDescriptionString()));
            //maps it by saying turn ProductDto.UnitOfMeasurement = Product.UnitOfMeasurement.ToDescriptionString()

        }
    }
}