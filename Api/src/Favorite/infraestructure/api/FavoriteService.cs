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
        
        public FavoriteService(ApplicationDBContext context)
        {
            getAllFavoritesService = new GetAllFavorites(context);
            createFavoriteService = new CreateFavorite(context);
        }

        public async Task<FavoriteDto> GetAllAsync(Guid userId)
        {
            return await getAllFavoritesService.Run(userId);
        }
        public async Task<CreatedFavoriteDto> CreateAsync(FavoriteEntity favorite)
        {
            return await createFavoriteService.Run(favorite);
        }
    }
}