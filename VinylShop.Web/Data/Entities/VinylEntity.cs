using Microsoft.EntityFrameworkCore;

namespace VinylShop.Web.Data.Entities
{
    public class VinylEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string Year { get; set; }
        public required Guid GenreId { get; set; }
        public GenreEntity? Genre { get; set; } = null!;
        public required string RecordLabel { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required byte[] Cover { get; set; }
    }
}
