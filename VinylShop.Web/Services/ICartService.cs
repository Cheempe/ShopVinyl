using VinylShop.Web.Models;
using VinylShop.Web.Models.Cart;

namespace VinylShop.Web.Services
{
    public interface ICartService
    {
        Task<CartModel> GetAsync(Guid userId, bool loadProducts = false, CancellationToken cancellationToken = default);
        Task AddToCartAsync(Guid userId, Guid productId, int quantity = 1, CancellationToken cancellationToken = default);
        Task RemoveFromCartAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default);
        Task AddCoverToCartItemAsync(Guid cartItemId, byte[] cover, CancellationToken cancellationToken = default);
        Task<ImageModel> GetCustomCoverAsync(Guid cartItemId, CancellationToken cancellationToken = default);
    }
}
