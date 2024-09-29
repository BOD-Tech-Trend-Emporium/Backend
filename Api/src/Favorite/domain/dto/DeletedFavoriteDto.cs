using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.src.Favorite.domain.dto
{
    public class DeletedFavoriteDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}