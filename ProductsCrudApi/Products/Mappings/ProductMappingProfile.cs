using AutoMapper;
using ProductsCrudApi.Products.Dto;
using ProductsCrudApi.Products.Model;

namespace ProductsCrudApi.Products.Mappings
{
    public class ProductMappingProfile:Profile
    {

        public ProductMappingProfile() {



            CreateMap<CreateProductRequest, Product>();
        }

    }
}
