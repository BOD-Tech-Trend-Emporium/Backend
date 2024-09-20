using Api.src.Product.application.mappers;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using backend.Data;
using backend.src.User.domain.enums;

namespace Api.src.Product.application.service
{
    public class CreateProduct
    {

        private readonly ApplicationDBContext _context;
        public CreateProduct(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ProductEntity> Run(CreateProductDto product, UserRole role)
        {
            var productModel = await ProductMappers.ToProductModelForCreate(product, _context, role) ;
            _context.Product.Add(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }
    }
}
