using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Common.exceptions;
using Api.src.Favorite.application.mappers;
using Api.src.Favorite.domain.dto;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Favorite.application.service
{
    public class DeleteByProductId
    {
        private readonly ApplicationDBContext _context;

        public DeleteByProductId(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<DeletedFavoriteDto?> Run(Guid productId, Guid userId)
        {
            var favoriteToDelete = await _context.Favorite.FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);

            if(favoriteToDelete == null)
            {
                throw new NotFoundException($"Favorite product with id {productId} not found in favorites for {userId}");
            }

            _context.Favorite.Remove(favoriteToDelete);
            await _context.SaveChangesAsync();

            return favoriteToDelete.ToDeletedFavoriteDto();
        }
    }
}