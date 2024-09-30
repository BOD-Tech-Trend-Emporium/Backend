using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Common.exceptions;
using Api.src.Favorite.application.mappers;
using Api.src.Favorite.application.validations;
using Api.src.Favorite.domain.dto;
using Api.src.Favorite.domain.entity;
using Api.src.Product.domain.entity;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Favorite.application.service
{
    public class CreateFavorite
    {
        private readonly ApplicationDBContext _context;
        private FavoriteValidations favoriteValidations;

        public CreateFavorite(ApplicationDBContext context)
        {
            _context = context;
            favoriteValidations = new FavoriteValidations(context);
        }
        public async Task<CreatedFavoriteDto> Run(FavoriteEntity favorite)
        {   
            bool isProductAlreadyAdded = await favoriteValidations.IsAlreadyAdded(favorite.ProductId, favorite.UserId);
            if(isProductAlreadyAdded)
            {
                throw new ConflictException("Product already added to favorites");
            }
            await _context.Favorite.AddAsync(favorite);
            await _context.SaveChangesAsync();
            CreatedFavoriteDto createdFavorite = FavoriteMappers.ToCreatedFavoriteDto(favorite);
            return createdFavorite;
        }
    }
}