namespace VinylShop.Web.Models.Product
{
    public class ProductModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string Year { get; set; }
        public required string Genre { get; set; }
        public required string RecordLabel { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
    }
}
