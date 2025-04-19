using Domain.Models;
using Shared;
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

        public ProductWithBrandAndTypeSpecifications (ProductSpecificationParamters specParams) : 
            base(
                   P => 
                   (string.IsNullOrEmpty(specParams.Sreach) || P.Name.ToLower().Contains(specParams.Sreach.ToLower()))&&
                   (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId) &&
                   (!specParams.TypeId.HasValue || P.TypeId == specParams.TypeId)
                )
        {
            ApplyInclude();

            ApplySorting(specParams.Sort);

            ApplyPagination(specParams.PageIndex, specParams.Pagesize);

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
