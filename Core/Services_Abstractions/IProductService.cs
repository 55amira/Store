using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_Abstractions
{
    public interface IProductService
    {
        // Get All Product
        //Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? BrandId, int? TypeId , string? sort, int pageIndex = 1, int pageSize = 5);
        Task<PaginationReponse<ProductResultDto>> GetAllProductsAsync(ProductSpecificationParamters specParams);

        // Get Product By Id
        Task<ProductResultDto?> GetProductByIdAsync(int id);

        // Get All Brands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();

        // Get All Types
        Task<IEnumerable<TypeResultDto>> GetAlltypesAsync();

    }
}
