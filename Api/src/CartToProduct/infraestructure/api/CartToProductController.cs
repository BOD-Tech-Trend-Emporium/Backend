using Api.src.Auth.application.Utils;
using Api.src.CartToProduct.application.mappers;
using Api.src.CartToProduct.application.service;
using Api.src.CartToProduct.domain.dto;
using Api.src.CartToProduct.domain.repository;
using Api.src.Category.domain.dto;
using Api.src.Category.domain.repository;
using backend.src.User.domain.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.CartToProduct.infraestructure.api
{
    [Route("api/cart/product")]
    [ApiController]
    public class CartToProductController : ControllerBase
    {
        private CartToProductRepository _cartToProductService;

        public CartToProductController(CartToProductRepository cartToProductService)
        {
            _cartToProductService = cartToProductService;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Shopper))]
        public async Task<IActionResult> Create([FromBody] CreateCartToProductDto createCartToProductDto)
        {
            var result = await _cartToProductService.CreateAsync(
                createCartToProductDto,
                Guid.Parse(Token.GetTokenPayload(Request).UserId));
            return Created($"/api/cart/product/", result.ToCartProductDto());
        }
    }
}
