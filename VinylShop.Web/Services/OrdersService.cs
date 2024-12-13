using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VinylShop.Web.Data;
using VinylShop.Web.Data.Entities;
using VinylShop.Web.Extensions;
using VinylShop.Web.Models;
using VinylShop.Web.Models.Orders;

namespace VinylShop.Web.Services
{
    public class OrdersService(AppDbContext context, IMailService mailService, UserManager<UserEntity> userManager) : IOrdersService
    {
        private readonly AppDbContext _context = context;
        private readonly IMailService _mailService = mailService;
        private readonly UserManager<UserEntity> _userManager = userManager;

        public async Task<OrderModel> CreateAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            UserEntity user = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new Exception("User not found");

            IQueryable<CartEntity> query = _context.Carts.AsNoTracking();
            query = query
                .Include(c => c.Items)
                .ThenInclude(c => c.Product)
                .ThenInclude(c => c.Genre);

            CartEntity? cart = await query.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
            if (cart is null)
            {
                cart = new() { UserId = userId };
                await _context.Carts.AddAsync(cart, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            OrderEntity order = new()
            {
                UserId = userId,
                Timestamp = DateTime.UtcNow
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.OrdersProduct.AddRangeAsync(cart.Items.Select(e => new OrderProductEntity()
            {
                OrderId = order.Id,
                ProductId = e.ProductId,
                Quantity = e.Quantity,
                CustomCover = e.CustomCover,
            }), cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var deleteCartItems = _context.CartItems.Where(c => c.CartId == cart.Id);
            _context.CartItems.RemoveRange(deleteCartItems);
            await _context.SaveChangesAsync(cancellationToken);

            await _mailService.SendAsync(
                new(user.Email), 
                new("vinylshop@gmail.com"), 
                "Order information", 
                $"Your order with ID \"{order.Id}\" has been sent for processing", 
                cancellationToken);

            OrderModel? result = await GetByIdAsync(order.Id, cancellationToken);
            return result!;
        }

        public async Task<List<OrderModel>> GetAllAsync(int skip = 0, int take = 12, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(e => e.Products)
                .ThenInclude(p => p.Product)
                .ThenInclude(e => e.Genre)
                .OrderByDescending(e => e.Timestamp)
                .Skip(skip)
                .Take(take)
                .Select(e => e.ToViewModel())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<OrderModel>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(e => e.Products)
                .ThenInclude(p => p.Product)
                .ThenInclude(e => e.Genre)
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Timestamp)
                .Select(e => e.ToViewModel())
                .ToListAsync(cancellationToken);
        }

        public async Task<OrderModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            OrderEntity? result = await _context.Orders
                .Include(e => e.Products)
                .ThenInclude(p => p.Product)
                .ThenInclude(e => e.Genre)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            return result?.ToViewModel();
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(e => e.Products)
                .ThenInclude(p => p.Product)
                .ThenInclude(e => e.Genre)
                .CountAsync(cancellationToken);
        }

        public async Task<ImageModel> GetCustomCoverAsync(Guid orderItemId, CancellationToken cancellationToken = default)
        {
            OrderProductEntity? result = await _context.OrdersProduct.FirstOrDefaultAsync(o => o.Id == orderItemId, cancellationToken)
                ?? throw new Exception("Order item not found");

            return new ImageModel() { Id = orderItemId, Content = result.CustomCover };
        }

        public async Task<OrderModel> UpdateAsync(Guid orderId, OrderStatus status, CancellationToken cancellationToken = default)
        {
            OrderEntity? result = await _context.Orders.FirstOrDefaultAsync(e => e.Id == orderId, cancellationToken)
                ?? throw new Exception("Order not found");

            result.Status = status;
            _context.Update(result);
            await _context.SaveChangesAsync(cancellationToken);
            return result.ToViewModel();
        }
    }
}
