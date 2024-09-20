using Api.src.Product.application.mappers;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.repository;
using Microsoft.AspNetCore.Mvc;

namespace Api.src.Product.infraestructure.api
{
    [Route("api/products")]
    [ApiController]
    public class ProductController: ControllerBase
    {
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

        // TODO Implement
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto product)
        {
            var created = await _productService.CreateAsync(product);
            return Ok(created);
        }
    }
}
