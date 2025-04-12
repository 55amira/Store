using AutoMapper;
using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product,ProductResultDto>()
                     .ForMember(p => p.BrandName , o => o.MapFrom( s => s.ProductBrand.Name))
                     .ForMember(p => p.TypeName, o => o.MapFrom(s => s.ProductType.Name))
                     ;
            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<ProductType, TypeResultDto>();


        }
    }
}
