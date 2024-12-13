using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using VinylShop.Web.Data;
using VinylShop.Web.Data.Entities;
using VinylShop.Web.Extensions;
using VinylShop.Web.Models;
using VinylShop.Web.Models.Cart;

namespace VinylShop.Web.Services
{
    public class CartService(AppDbContext context) : ICartService
    {
        private readonly AppDbContext _context = context;

        public async Task AddCoverToCartItemAsync(Guid cartItemId, byte[] cover, CancellationToken cancellationToken = default)
        {
            CartItemEntity? cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.Id == cartItemId, cancellationToken: cancellationToken)
                ?? throw new Exception("Cart item not found");

            cartItem.CustomCover = cover;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddToCartAsync(Guid userId, Guid productId, int quantity = 1, CancellationToken cancellationToken = default)
        {
            CartEntity cart = await GetCartEntityAsync(userId, cancellationToken: cancellationToken);
            CartItemEntity? cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem is null)
            {
                cartItem = new()
                {
                    ProductId = productId,
                    CartId = cart.Id,
                    Quantity = quantity
                };
                cart.Items.Add(cartItem);
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CartModel> GetAsync(Guid userId, bool loadProducts = false, CancellationToken cancellationToken = default)
        {
            CartEntity cart = await GetCartEntityAsync(userId, loadProducts, cancellationToken: cancellationToken);
            return cart.ToViewModel();
        }

        public async Task<ImageModel> GetCustomCoverAsync(Guid cartItemId, CancellationToken cancellationToken = default)
        {
            CartItemEntity? cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.Id == cartItemId, cancellationToken: cancellationToken)
                ?? throw new Exception("Cart item not found");

            return new ImageModel() { Id = cartItemId, Content = cartItem.CustomCover };
        }

        public async Task RemoveFromCartAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default)
        {
            CartEntity cart = await GetCartEntityAsync(userId, cancellationToken: cancellationToken);
            CartItemEntity? cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem is null)
                return;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<CartEntity> GetCartEntityAsync(Guid userId, bool loadProducts = false, CancellationToken cancellationToken = default)
        {
            IQueryable<CartEntity> query = _context.Carts.AsNoTracking();
            if (loadProducts)
                query = query
                    .Include(c => c.Items)
                    .ThenInclude(c => c.Product)
                    .ThenInclude(c => c.Genre);
            else
                query = query
                    .Include(c => c.Items);

            CartEntity? cart = await query.FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
            if (cart is not null)
                return cart;

            cart = new() { UserId = userId };
            await _context.Carts.AddAsync(cart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return cart;
        }
    }
}
