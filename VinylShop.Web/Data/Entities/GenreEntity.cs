namespace VinylShop.Web.Data.Entities
{
    public class GenreEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public virtual ICollection<VinylEntity> Vinyls { get; set; } = [];
    }
}
