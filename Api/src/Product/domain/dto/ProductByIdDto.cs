namespace Api.src.Product.domain.dto
{
    public class ProductByIdDto  : ProductDto
    {
        public InventoryAuxDto Inventory {  get; set; }

    }
    public class InventoryAuxDto
    {
        public int Total { get; set; }
        public int Available { get; set; }
    }
}
