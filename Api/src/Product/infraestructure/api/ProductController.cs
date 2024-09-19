using Api.src.Product.application.mappers;
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
        public async Task<IActionResult>GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
    }
}
