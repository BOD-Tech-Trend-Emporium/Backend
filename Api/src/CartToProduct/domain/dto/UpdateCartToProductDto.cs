namespace Api.src.CartToProduct.domain.dto
{
    public class UpdateCartToProductDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
