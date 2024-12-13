namespace VinylShop.Web.Data.Entities
{
    public class OrderProductEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid OrderId { get; set; }
        public OrderEntity Order { get; set; } = null!;
        public required Guid ProductId { get; set; }
        public VinylEntity Product { get; set; } = null!;
        public int Quantity { get; set; }
        public byte[]? CustomCover { get; set; }
    }
}
