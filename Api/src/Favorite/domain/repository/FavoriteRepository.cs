using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Favorite.domain.dto;
using Api.src.Favorite.domain.entity;

namespace Api.src.Favorite.domain.repository
{
    public interface FavoriteRepository
    {
        Task<FavoriteDto> GetAllAsync(Guid userId);
        Task<CreatedFavoriteDto> CreateAsync(FavoriteEntity favorite);
        Task<DeletedFavoriteDto?> DeleteByProductIdAsync(Guid productId, Guid userId);
    }
}