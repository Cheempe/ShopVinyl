using VinylShop.Web.Data.Entities;
using VinylShop.Web.Models;
using VinylShop.Web.Models.Orders;

namespace VinylShop.Web.Services
{
    public interface IOrdersService
    {
        Task<List<OrderModel>> GetAllAsync(int skip = 0, int take = 12, CancellationToken cancellationToken = default);
        Task<List<OrderModel>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<OrderModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<OrderModel> CreateAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(CancellationToken cancellationToken = default);
        Task<OrderModel> UpdateAsync(Guid orderId, OrderStatus status, CancellationToken cancellationToken = default);
        Task<ImageModel> GetCustomCoverAsync(Guid orderItemId, CancellationToken cancellationToken = default);
    }
}
