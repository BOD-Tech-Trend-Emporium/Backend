using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Product.domain.dto;

namespace Api.src.Favorite.domain.dto
{
    public class CreatedFavoriteDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

    }
}