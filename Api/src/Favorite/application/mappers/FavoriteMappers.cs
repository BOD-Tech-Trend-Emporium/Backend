using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Favorite.domain.dto;
using Api.src.Favorite.domain.entity;
using Api.src.Product.domain.dto;

namespace Api.src.Favorite.application.mappers
{
    public static class FavoriteMappers
    {
        public static FavoriteEntity ToFavoriteModelForCreate(Guid productId, Guid userId)
        {
            return new FavoriteEntity
            {
                ProductId = productId,
                UserId = userId
            };
        }

        public static CreatedFavoriteDto ToCreatedFavoriteDto(FavoriteEntity favorite)
        {
            return new CreatedFavoriteDto
            {
                UserId = favorite.UserId,
                ProductId = favorite.ProductId
            };
        }

        public static DeletedFavoriteDto ToDeletedFavoriteDto(this FavoriteEntity favorite)
        {
            return new DeletedFavoriteDto
            {
                UserId = favorite.UserId,
                ProductId = favorite.ProductId
            };
        }
    }
}