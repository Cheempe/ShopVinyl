using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using VinylShop.Web.Data;
using VinylShop.Web.Data.Entities;
using VinylShop.Web.Extensions;
using VinylShop.Web.Models;
using VinylShop.Web.Models.Product;
using VinylShop.Web.Services;

namespace VinylShop.Web.Controllers
{
    public class ProductsController(IProductsService productsService, ICartService cartService, AppDbContext context) : Controller
    {
        private readonly string _productsSettingsSessionKey = "products-settings";
        private readonly IProductsService _productsService = productsService;
        private readonly ICartService _cartService = cartService;
        private readonly AppDbContext _context = context;

        [Route("Products/Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            //if (item == null)
            //{
            //    return View("Error", "Product not found");
            //}
            await _productsService.DeleteAsync(id, cancellationToken);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index(int? page, List<string>? selectedGenres = null, string? name = null, decimal? minPrice = null, decimal? maxPrice = null ,bool clear = false, CancellationToken cancellationToken = default)
        {
            ProductsViewSettings settings = HttpContext.Session.Get<ProductsViewSettings>(_productsSettingsSessionKey) ?? new();
            if (clear)
                settings.Filter = new();

            if (!string.IsNullOrEmpty(name))
                settings.Filter.Name = name;

            if (selectedGenres is not null)
            {
                Dictionary<string, bool> genre = _context.Genre
                .Select(v => v.Name)
                .ToDictionary(x => x, x => false);

                foreach (var item in selectedGenres)
                    genre[item] = true;

                settings.Filter.SelectedGenres = genre;
            }

            if (page is not null)
                settings.Pagination.CurrentPage = page.Value;

            if (settings.Filter.SelectedGenres is null)
                settings.Filter.SelectedGenres = _context.Genre
                    .Select(v => v.Name)
                    .ToDictionary(x => x, x => false);

            List<string>? selectedGenresList = settings.Filter.SelectedGenres?
                .Where(g => g.Value == true)
                .Select(g => g.Key)
                .ToList();

            if (minPrice is not null)
                settings.Filter.MinPrice = minPrice.Value;

            if (maxPrice is not null)
                settings.Filter.MaxPrice = maxPrice.Value;

            settings.Pagination.TotalItems = await _productsService.GetCountAsync(selectedGenresList, settings.Filter.Name, settings.Filter.MinPrice, settings.Filter.MaxPrice, cancellationToken);
            HttpContext.Session.Set(_productsSettingsSessionKey, settings);
            ViewBag.PageSettings = settings;
            ViewBag.Cart = await _cartService.GetAsync(User.Id().Value, true, cancellationToken);

            List<LiteProductModel> result = await _productsService.GetAllAsync(
                (settings.Pagination.CurrentPage - 1) * settings.Pagination.ItemsPerPage,
                settings.Pagination.ItemsPerPage,
                selectedGenresList,
                settings.Filter.Name,
                settings.Filter.MinPrice,
                settings.Filter.MaxPrice,
                cancellationToken);
            return View(result);
        }

        [Route("Products/{id}")]
        public async Task<IActionResult> Open(Guid id, CancellationToken cancellationToken = default)
        {
            ProductModel? item = await _productsService.GetByIdAsync(id, cancellationToken);
            ViewBag.Cart = await _cartService.GetAsync(User.Id().Value, true, cancellationToken);
            if (item == null)
            {
                return View("Error", "Product not found");
            }
            return View("Open", item);
        }

        // Добавление товара в корзину
        public async Task<IActionResult> AddToCart(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _productsService.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                return NotFound();
            }

            return RedirectToAction("AddToCart", "Cart", new { productId = id });
        }
    }
}
