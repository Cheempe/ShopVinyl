using VinylShop.Web.Models.Product;

namespace VinylShop.Web.Models.Orders
{
    public class OrdersViewSettings
    {
        public PaginationInfo Pagination { get; set; } = new();
        //public OrdersFilter Filter { get; set; } = new();
    }
}
