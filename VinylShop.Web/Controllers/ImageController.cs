using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VinylShop.Web.Models;
using VinylShop.Web.Services;

namespace VinylShop.Web.Controllers
{
    [Route("image")]
    public class ImageController(IProductsService productsService, ICartService cartService, IOrdersService ordersService) : Controller
    {
        private readonly IProductsService _productsService = productsService;
        private readonly ICartService _cartService = cartService;
        private readonly IOrdersService _ordersService = ordersService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(Guid id, CancellationToken cancellationToken = default)
        {
            ImageModel imageModel = await _productsService.GetProductImageAsync(id, cancellationToken);
            if (imageModel == null)
                return NotFound();

            return File(imageModel.Content, "image/jpeg");
        }

        [HttpGet("custom/{id}")]
        public async Task<IActionResult> GetCustomImage(Guid id, bool isOrder = false, CancellationToken cancellationToken = default)
        {
            ImageModel imageModel;
            if (!isOrder)
                imageModel = await _cartService.GetCustomCoverAsync(id, cancellationToken);
            else
                imageModel = await _ordersService.GetCustomCoverAsync(id, cancellationToken);

            if (imageModel == null)
                return NotFound();

            return File(imageModel.Content, "image/jpeg");
        }
    }

}
