using Api.src.Product.application.service;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.repository;
using backend.Data;

namespace Api.src.Product.infraestructure.api
{
    public class ProductService : ProductRepository
    {
        private GetAllProducts getAllProductsService; 
        public ProductService(ApplicationDBContext context) 
        {
            getAllProductsService = new GetAllProducts(context);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await getAllProductsService.Run();
        }

        public Task<bool> CreateAsync(CreateProductDto givenProduct)
        {
            throw new NotImplementedException();
        }

        public Task<ProductEntity> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductEntity> UpdateByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}