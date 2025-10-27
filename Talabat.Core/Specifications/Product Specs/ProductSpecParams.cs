using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 10;

        private int pageSize = 5;
        private string? search;

        public int PageSize { get => pageSize; set => pageSize = value > MaxPageSize? MaxPageSize:value ; }

        public int PageIndex { get; set; } = 1;

        public string? sort { get; set; }

        public int? brandId { get; set; }

        public int? categoryId { get; set; }

        public string? Search { get => search; set => search = value?.ToLower(); }

    }
}
