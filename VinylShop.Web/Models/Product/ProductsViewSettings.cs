namespace VinylShop.Web.Models.Product
{
    public class ProductsViewSettings
    {
        public PaginationInfo Pagination { get; set; } = new();
        public ProductsFilter Filter { get; set; } = new();
    }
}
