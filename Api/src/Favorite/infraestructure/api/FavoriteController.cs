using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.src.Auth.application.Utils;
using Api.src.Favorite.application.mappers;
using Api.src.Favorite.domain.dto;
using Api.src.Favorite.domain.repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Favorite.infraestructure.api
{   
    [Route("api/user/wishlist")]
    [ApiController]
    public class FavoriteController: ControllerBase
    {
        private FavoriteRepository _favoriteService;
        
        public FavoriteController(FavoriteRepository favoriteRepository)
        {
            _favoriteService = favoriteRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            Guid userId = Guid.Parse(Token.GetTokenPayload(Request).UserId);
            var favoritesList = await _favoriteService.GetAllAsync(userId);
            return Ok(favoritesList);
        }

        [HttpPost]
        [Route("add/{productId}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] Guid productId)
        {
            Guid userId = Guid.Parse(Token.GetTokenPayload(Request).UserId);
            var favoriteModel = FavoriteMappers.ToFavoriteModelForCreate(productId, userId);
            CreatedFavoriteDto createdFavorite = await _favoriteService.CreateAsync(favoriteModel);
            return Created($"/api/user/wishlist/{favoriteModel.ProductId}", createdFavorite);
        }

        [HttpDelete]
        [Route("remove/{productId}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            Guid userId = Guid.Parse(Token.GetTokenPayload(Request).UserId);
            var deletedFavorite = await _favoriteService.DeleteByProductIdAsync(productId, userId);
            return Ok(deletedFavorite);
        }
    }
}