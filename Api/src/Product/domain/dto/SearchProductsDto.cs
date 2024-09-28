using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Product.domain.dto
{
    public class SearchProductsDto
    {
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public float MinPrice { get; set; } = 0;
        public float MaxPrice { get; set; } = float.MaxValue;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}