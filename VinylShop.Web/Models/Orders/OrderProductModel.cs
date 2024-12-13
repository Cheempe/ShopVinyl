using VinylShop.Web.Data.Entities;
using VinylShop.Web.Models.Product;

namespace VinylShop.Web.Models.Orders
{
    public class OrderProductModel
    {
        public required Guid Id { get; set; }
        public required ProductModel Product { get; set; }
        public required int Quantity { get; set; }
        public decimal TotalPrice => Product.Price * Quantity;
        public byte[]? CustomCover { get; set; }
    }
}
