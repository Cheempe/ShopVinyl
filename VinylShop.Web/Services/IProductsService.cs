using VinylShop.Web.Models;
using VinylShop.Web.Models.Product;

namespace VinylShop.Web.Services
{
    public interface IProductsService
    {
        Task<List<LiteProductModel>> GetAllAsync(
            int skip = 0, 
            int take = 12, 
            List<string>? selectedGenres = null, 
            string? name = null, 
            decimal? minPrice = null,
            decimal? maxPrice = null,
            CancellationToken cancellationToken = default);
        Task<ProductModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(CreateProductModel model, CancellationToken cancellationToken = default);
        Task<ProductModel> UpdateAsync(UpdateProductModel model, CancellationToken cancellationToken = default);
        Task<ImageModel> GetProductImageAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(
            List<string>? selectedGenres = null, 
            string? name = null, 
            decimal? minPrice = null,
            decimal? maxPrice = null, 
            CancellationToken cancellationToken = default);
    }
}
