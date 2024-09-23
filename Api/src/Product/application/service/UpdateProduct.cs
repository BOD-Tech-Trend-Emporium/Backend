using Api.src.Common.exceptions;
using Api.src.Price.domain.entity;
using Api.src.Product.domain.dto;
using Api.src.Product.domain.entity;
using Api.src.Product.domain.enums;
using backend.Data;
using Microsoft.EntityFrameworkCore;

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
            var productEntity = await _context.Product
                .Where(pr => pr.Id == productId)
                .Include(p => p.Category) // Load related category
                .Include(p => p.Prices) // Load related prices
                .Include(p => p.Reviews) // Load reviews related 
                .FirstOrDefaultAsync();
            if (productEntity == null)
            {
                throw new NotFoundException($"Product with Id {productId} not found");
            }
            // Updating values
            // TODO: Validate the availability and the stock
            productEntity.Stock = product.Stock;
            productEntity.Title = product.Title;
            productEntity.Description = product.Description;
            productEntity.Image = product.Image;
            // update the category
            if(productEntity.Category.Id != product.Category)
            {
                var newCategory = await _context.Category.FirstOrDefaultAsync(cat => cat.Id == product.Category);
                if (newCategory == null)
                {
                    throw new NotFoundException($"Category with name {product.Category} NotFound");
                }
                productEntity.Category = newCategory;
            }
            // update the price
            var currPrice = productEntity.Prices?.Where(pr => pr.Current).FirstOrDefault();
            if (currPrice == null)
            {
                var newPrice = this.newPrice(productEntity, product.Price, true);
                productEntity.Prices.Add(newPrice);
            }else if (currPrice.Price != product.Price)
            {
                currPrice.Current = false;
                var newPrice = this.newPrice(productEntity, product.Price, true);
                productEntity.Prices.Add(newPrice);
            }
            
            await _context.SaveChangesAsync();

            return productEntity;
        }
        private PriceEntity newPrice(ProductEntity product, float price, bool current)
        {
            return new PriceEntity
            {
                Price = price,
                Current = current,
                Product = product,
            };
        }


    }
}
