using VinylShop.Web.Data.Entities;

namespace VinylShop.Web.Models.Cart
{
    public class CartModel
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public List<CartItemModel> Items { get; set; } = [];
        public decimal TotalPrice => Items.Sum(x => x.TotalPrice);
    }
}
