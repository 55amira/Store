using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Specifications;
using Services_Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService (IUnitOfWork unitOfWork , IMapper mapper) : IProductService
    {
        
        public async Task<PaginationReponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParamters specParams)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(specParams);
            

            // Get All Products Throught ProductRepository 
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);

            var specCount = new ProductWithCountSpecification(specParams);

            var count = await unitOfWork.GetRepository<Product,int>().CountAsync(specCount);



            // Mapping IEnumerable<Product> To IEnumerable<ProductResultDto>: Automapper 
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return new PaginationReponse<ProductResultDto>(specParams.PageIndex,specParams.Pagesize,count,result);
        }
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) throw new ProductNotFoundException(id);
            var result = mapper.Map<ProductResultDto>(product);
            return result;
            
        }
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<BrandResultDto>>(brands);
            return result; 
        }

        public async Task<IEnumerable<TypeResultDto>> GetAlltypesAsync()
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var result = mapper.Map<IEnumerable<TypeResultDto>>(Types);

            return result;
        }

        
    }
}
