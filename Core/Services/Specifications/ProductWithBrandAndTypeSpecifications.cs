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

        public ProductWithBrandAndTypeSpecifications (int? BrandId, int? TypeId ,string? sort) : 
            base(
                   P => 
                   (!BrandId.HasValue || P.BrandId == BrandId)&&
                   (!TypeId.HasValue || P.TypeId == TypeId)
                )
        {
            ApplyInclude();

            ApplySorting(sort);

        }

       private void ApplyInclude ()
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

        }
        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }

    }
}
