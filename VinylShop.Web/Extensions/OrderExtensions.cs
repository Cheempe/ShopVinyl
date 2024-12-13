using VinylShop.Web.Data.Entities;
using VinylShop.Web.Models.Orders;

namespace VinylShop.Web.Extensions
{
    public static class OrderExtensions
    {
        public static OrderModel ToViewModel(this OrderEntity src)
        {
            return new OrderModel()
            {
                Id = src.Id,
                Products = src.Products.Select(p => p.ToViewModel()),
                UserId = src.UserId,
                Timestamp = src.Timestamp,
                Status = src.Status,
            };
        }

        public static OrderProductModel ToViewModel(this OrderProductEntity src)
        {
            return new OrderProductModel()
            {
                Id = src.Id,
                Product = src.Product.ToViewModel(),
                Quantity = src.Quantity,
                CustomCover = src.CustomCover,
            };
        }
    }
}
