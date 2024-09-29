using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Api.src.Favorite.application.mappers;
using Api.src.Favorite.domain.dto;
using Api.src.Favorite.domain.entity;
using Api.src.Product.domain.dto;
using backend.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.src.Favorite.application.service
{
    public class GetAllFavorites
    {
        private readonly ApplicationDBContext _context;

        public GetAllFavorites(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<FavoriteDto> Run(Guid userId)
        {
            var favorites = await _context.Favorite
                .Where(x => x.UserId == userId)
                .Include(x => x.Product)
                .Include(x => x.Product.Category)
                .ToListAsync();

            var favoritesList = new FavoriteDto
            {
                UserId = userId,
                Products = favorites.Select(x => new AddedToFavoriteProductDto
                {
                   Title = x.Product.Title,
                   Price = x.Product.Price,
                   Description = x.Product.Description,
                   Category = x.Product.Category.Name,
                   Image = x.Product.Image 
                }).ToList()
            };
            return favoritesList;
        }
    }
}