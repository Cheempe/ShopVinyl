namespace VinylShop.Web.Models.Product
{
    public class ProductsFilter
    {
        public string? Name { get; set; }
        public Dictionary<string, bool>? SelectedGenres { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
