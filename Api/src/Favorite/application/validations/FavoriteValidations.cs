using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Favorite.application.validations
{
    public class FavoriteValidations
    {
        private readonly ApplicationDBContext _context;
        public FavoriteValidations(ApplicationDBContext context){
            _context = context;
        }

        public async Task<bool> IsAlreadyAdded(Guid productId, Guid userId)
        {
            return await _context.Favorite.AnyAsync(x => x.ProductId == productId && x.UserId == userId);
        }
    }
}