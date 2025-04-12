using Services_Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        public ProductService( )
        { 
        }
        public Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }
        public Task<ProductResultDto> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BrandResultDto>> GetAlltypesAsync()
        {
            throw new NotImplementedException();
        }

        
    }
}
