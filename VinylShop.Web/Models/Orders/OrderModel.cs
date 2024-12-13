using VinylShop.Web.Data.Entities;
using VinylShop.Web.Helpers;

namespace VinylShop.Web.Models.Orders
{
    public class OrderModel
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public IEnumerable<OrderProductModel> Products { get; set; } = [];
        public required DateTime Timestamp { get; set; }
        public required OrderStatus Status { get; set; }
        public string StatusString => EnumHelper.GetDisplayName(Status);
        public decimal TotalPrice => Products.Select(p => p.Product).Sum(p => p.Price);
    }
}
