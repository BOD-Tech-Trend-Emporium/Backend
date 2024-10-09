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
        private SearchProducts searchProductsService;
        private CreateProduct createProductService;
        private GetProductById getProductByIdService;
        private UpdateProduct updateProductService;
        private GetThreeLatestProducts getThreeLatestProductsService;
        public ProductService(ApplicationDBContext context) 
        {
            getAllProductsService = new GetAllProducts(context);
            searchProductsService = new SearchProducts(context);
            createProductService = new CreateProduct(context);
            getProductByIdService = new GetProductById(context);
            updateProductService = new UpdateProduct(context);
            getThreeLatestProductsService = new GetThreeLatestProducts(context);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await getAllProductsService.Run();
        }

        public async Task<List<ProductDto>> SearchAsync(SearchProductsDto query)
        {
            return await searchProductsService.Run(query);
        }

        public async Task<ProductEntity> CreateAsync(CreateProductDto givenProduct, UserRole role)
        {
            return await createProductService.Run(givenProduct, role);
        }

        public Task<ProductEntity> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductByIdDto> GetByIdAsync(Guid id)
        {
            return await getProductByIdService.Run(id);
        }

        public async Task<ProductEntity> UpdateByIdAsync(CreateProductDto product, Guid id)
        {
            return await updateProductService.Run(product, id);
        }

        public async Task<List<ProductDto>> GetThreeLatestAsync()
        {
            return await getThreeLatestProductsService.Run();
        }
    }
}