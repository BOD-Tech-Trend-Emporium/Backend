using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using backend.Data;

namespace Api.src.Product.application.service
{
    public class UpdateProduct
    {
        private readonly ApplicationDBContext _context;

        public UpdateProduct(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ProductEntity> Run(CreateProductDto product, Guid productId)
        {
            return null;
        }


    }
}
