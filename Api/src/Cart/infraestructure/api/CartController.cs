using Api.src.Auth.application.Utils;
using Api.src.Cart.application.mappers;
using Api.src.Cart.domain.dto;
using Api.src.Cart.domain.repository;
using Api.src.Category.domain.dto;
using Api.src.Category.domain.repository;
using Azure.Core;
using backend.src.User.domain.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Cart.infraestructure.api
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private CartRepository _cartRepository;

        public CartController(CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Shopper))]
        public async Task<IActionResult> Create()
        {
            var result = await _cartRepository.CreateAsync(Guid.Parse(Token.GetTokenPayload(Request).UserId));
            return Created($"/api/cart/{result.User.Id}", result.ToCreateCartResponseDto());
        }
    }
}
