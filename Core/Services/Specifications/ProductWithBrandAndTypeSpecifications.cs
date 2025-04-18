using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product,int>
    {
        public ProductWithBrandAndTypeSpecifications (int id) :base (p => p.Id ==id)
        {
            ApplyInclude();


        }

        public ProductWithBrandAndTypeSpecifications (int? BrandId, int? TypeId) : 
            base(
                   P => 
                   (!BrandId.HasValue || P.BrandId == BrandId)&&
                   (!TypeId.HasValue || P.TypeId == TypeId)
                )
        {
            ApplyInclude();

        }

       private void ApplyInclude ()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

        }

    }
}
