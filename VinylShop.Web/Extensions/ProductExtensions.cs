using VinylShop.Web.Data.Entities;
using VinylShop.Web.Models.Product;

namespace VinylShop.Web.Extensions
{
    public static class ProductExtensions
    {
        public static ProductModel ToViewModel(this VinylEntity src)
        {
            return new()
            {
                Author = src.Author,
                Genre = src.Genre!.Name,
                Id = src.Id,
                Name = src.Name,
                Price = src.Price,
                RecordLabel = src.RecordLabel,
                Description = src.Description,
                Year = src.Year,
            };
        }
    }
}
