using AutoMapper;
using Domain.Contracts;
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
        
        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? BrandId, int? TypeId, string? sort , int pageIndex = 1, int pageSize = 5)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(BrandId, TypeId, sort , pageIndex, pageSize);
            

            // Get All Products Throught ProductRepository 
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);



            // Mapping IEnumerable<Product> To IEnumerable<ProductResultDto>: Automapper 
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return result;
        }
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(spec);
            if (product is null) return null;
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
