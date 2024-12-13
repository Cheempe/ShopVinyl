namespace VinylShop.Web.Models.Product
{
    public class LiteProductModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string Genre { get; set; }
        public required decimal Price { get; set; }
    }
}
