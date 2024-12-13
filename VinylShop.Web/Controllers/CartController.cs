using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylShop.Web.Extensions;
using VinylShop.Web.Models.Cart;
using VinylShop.Web.Services;

namespace VinylShop.Web.Controllers
{
    [Authorize]
    public class CartController(ICartService service) : Controller
    {
        private readonly ICartService _service = service;

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            Guid? userId = User.Id();
            CartModel cart = await _service.GetAsync(userId.Value, true, cancellationToken);
            return View(cart);
        }

        public async Task<IActionResult> AddToCartAsync(Guid productId, int quantity = 1, CancellationToken cancellationToken = default)
        {
            Guid? userId = User.Id();
            await _service.AddToCartAsync(userId.Value, productId, quantity, cancellationToken);
            return Ok();
        }

        public async Task<IActionResult> RemoveFromCartAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            Guid? userId = User.Id();
            await _service.RemoveFromCartAsync(userId.Value, productId, cancellationToken);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddCustomCoverAsync(Guid cartItemId, IFormFile file, CancellationToken cancellationToken = default)
        {
            byte[] coverBytes;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                coverBytes = ms.ToArray();
            }

            await _service.AddCoverToCartItemAsync(cartItemId, coverBytes, cancellationToken);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCustomCoverAsync(Guid cartItemId, CancellationToken cancellationToken = default)
        {
            await _service.AddCoverToCartItemAsync(cartItemId, null, cancellationToken);
            return RedirectToAction("Index");
        }
    }
}
