namespace VinylShop.Web.Models
{
    public class ImageModel
    {
        public required Guid Id { get; set; }
        public byte[]? Content { get; set; }
    }
}
