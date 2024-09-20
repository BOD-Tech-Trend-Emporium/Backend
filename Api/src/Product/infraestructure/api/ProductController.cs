using Api.src.Product.application.mappers;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.repository;
using backend.src.User.domain.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Product.infraestructure.api
{
    [Route("api/products")]
    [ApiController]
    public class ProductController: ControllerBase
    {
        // TODO: Update Swagger documentation 

        private ProductRepository _productService;

        public ProductController(ProductRepository productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        [ProducesResponseType(200, Type =typeof(List<ProductDto>))]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Employee)}")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto product)
        {   
            // TODO CHANGE DEFAULT ADMIN ROLE TO JWT ROLE
            var created = await _productService.CreateAsync(product, UserRole.Admin);
            var productId = created.Id;
            return Created($"/api/products/{productId}",productId);
        }
    }
}
