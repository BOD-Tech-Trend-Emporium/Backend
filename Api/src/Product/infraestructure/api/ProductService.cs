using Api.src.Product.application.service;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.repository;
using backend.Data;
using backend.src.User.domain.enums;

namespace Api.src.Product.infraestructure.api
{
    public class ProductService : ProductRepository
    {
        private GetAllProducts getAllProductsService;
        private CreateProduct createProductService;
        private UpdateProduct updateProductService;
        public ProductService(ApplicationDBContext context) 
        {
            getAllProductsService = new GetAllProducts(context);
            createProductService = new CreateProduct(context);
            updateProductService = new UpdateProduct(context);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await getAllProductsService.Run();
        }

        public async Task<ProductEntity> CreateAsync(CreateProductDto givenProduct, UserRole role)
        {
            return await createProductService.Run(givenProduct, role);
        }

        public Task<ProductEntity> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductEntity> UpdateByIdAsync(CreateProductDto product, Guid id)
        {
            return await updateProductService.Run(product, id);
        }
    }
}