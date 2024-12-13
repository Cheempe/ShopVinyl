using VinylShop.Web.Data.Entities;
using VinylShop.Web.Models.Cart;

namespace VinylShop.Web.Extensions
{
    public static class CartExtensions
    {
        public static CartModel ToViewModel(this CartEntity src)
        {
            CartModel model = new()
            {
                Id = src.Id,
                UserId = src.UserId,
                Items = src.Items.Select(i => i.ToViewModel()).ToList()
            };

            return model;
        }

        public static CartItemModel ToViewModel(this CartItemEntity src)
        {
            return new()
            {
                Id = src.Id,
                CartId = src.CartId,
                Product = src.Product?.ToViewModel(),
                Quantity = src.Quantity,
                CustomCover = src.CustomCover
            };
        }
    }
}
