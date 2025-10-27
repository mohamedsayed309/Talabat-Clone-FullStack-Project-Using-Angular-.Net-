using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithFilterationForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpecifications(ProductSpecParams specParams)
            :base(p =>
            (string.IsNullOrEmpty(specParams.Search) || p.Name.Contains(specParams.Search)) &&
            (!specParams.brandId.HasValue || p.BrandId == specParams.brandId.Value) &&
            (!specParams.categoryId.HasValue || p.CategoryId == specParams.categoryId.Value)
            )
        {
            
        }
    }
}
