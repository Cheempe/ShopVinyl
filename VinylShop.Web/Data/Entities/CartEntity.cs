namespace VinylShop.Web.Data.Entities
{
    public class CartEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid UserId { get; set; }
        public ICollection<CartItemEntity> Items { get; set; } = [];
    }
}
