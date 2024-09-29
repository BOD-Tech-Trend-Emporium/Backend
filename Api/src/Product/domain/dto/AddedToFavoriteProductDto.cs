using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Product.domain.dto
{
    public class AddedToFavoriteProductDto
    {
        public required string Title { get; set; }
        public float Price { get; set; }
        public required string Description { get; set; }
        public string Category { get; set; }
        public required string Image { get; set; }
    }
}