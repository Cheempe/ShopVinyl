using System.ComponentModel.DataAnnotations;

namespace VinylShop.Web.Data.Entities
{
    public class CartItemEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid CartId { get; set; }
        public CartEntity? Cart { get; set; } = null!;
        public required Guid ProductId { get; set; }
        public VinylEntity? Product { get; set; } = null!;
        public int Quantity { get; set; }
        public byte[]? CustomCover { get; set; }
    }
}
