namespace Api.src.CartToProduct.domain.dto
{
    public class CreateCartToProductDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
