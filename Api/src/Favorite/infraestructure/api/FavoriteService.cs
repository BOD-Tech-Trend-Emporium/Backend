using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Favorite.application.service;
using Api.src.Favorite.domain.dto;
using Api.src.Favorite.domain.entity;
using Api.src.Favorite.domain.repository;
using backend.Data;

namespace Api.src.Favorite.infraestructure.api
{
    public class FavoriteService : FavoriteRepository
    {
        private GetAllFavorites getAllFavoritesService;
        private CreateFavorite createFavoriteService;
        private DeleteByProductId deleteByProductIdService;
        
        public FavoriteService(ApplicationDBContext context)
        {
            getAllFavoritesService = new GetAllFavorites(context);
            createFavoriteService = new CreateFavorite(context);
            deleteByProductIdService = new DeleteByProductId(context);
        }

        public async Task<FavoriteDto> GetAllAsync(Guid userId)
        {
            return await getAllFavoritesService.Run(userId);
        }
        public async Task<CreatedFavoriteDto> CreateAsync(FavoriteEntity favorite)
        {
            return await createFavoriteService.Run(favorite);
        }

        public async Task<DeletedFavoriteDto?> DeleteByProductIdAsync(Guid productId, Guid userId)
        {
            return await deleteByProductIdService.Run(productId, userId);
        }
    }
}