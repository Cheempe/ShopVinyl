namespace VinylShop.Web.Models.Product
{
    public class CreateProductModel
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Year { get; set; }
        public Guid? GenreId { get; set; }
        public string? RecordLabel { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public IFormFile? Cover { get; set; }
    }
}
