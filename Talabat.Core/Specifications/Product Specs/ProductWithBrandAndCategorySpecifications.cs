using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(p =>
            (string.IsNullOrEmpty(specParams.Search) || p.Name.Contains(specParams.Search))&&
            (!specParams.brandId.HasValue||p.BrandId== specParams.brandId.Value) &&
            (!specParams.categoryId.HasValue || p.CategoryId == specParams.categoryId.Value)
            )
        {
            AddIncudes();
            if (!string.IsNullOrEmpty(specParams.sort))
            {
                switch (specParams.sort)
                {
                    case "priceAsc":
                        //OrderBy = p=>p.Price; or 
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        //OrderByDesc = p=>p.Price; or 
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
                AddOrderBy(p => p.Name);

            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

        }

        public ProductWithBrandAndCategorySpecifications(int id)
            :base(P => P.Id == id)
        {
            AddIncudes();
        }
        private void AddIncudes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
            
        }
    }
}
