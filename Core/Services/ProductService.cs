using AutoMapper;
using Domain.Contracts;
using Domain.Models;
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
        
        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            // Get All Products Throught ProductRepository 
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync();

            // Mapping IEnumerable<Product> To IEnumerable<ProductResultDto>: Automapper 
            var result = mapper.Map<IEnumerable<ProductResultDto>>(products);

            return result;
        }
        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(id);
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
