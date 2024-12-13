using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using VinylShop.Web.Data;
using VinylShop.Web.Data.Entities;
using VinylShop.Web.Extensions;
using VinylShop.Web.Helpers;
using VinylShop.Web.Models;
using VinylShop.Web.Models.Orders;
using VinylShop.Web.Models.Product;
using VinylShop.Web.Services;

namespace VinylShop.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController(IProductsService productsService, IOrdersService ordersService, UserManager<UserEntity> userManager, AppDbContext context) : Controller
    {
        private readonly string _productsAdminSettingsSessionKey = "admin-products-settings";
        private readonly string _ordersAdminSettingsSessionKey = "admin-orders-settings";
        private readonly string _usersAdminSettingsSessionKey = "users-orders-settings";
        private readonly IProductsService _productsService = productsService;
        private readonly IOrdersService _ordersService = ordersService;
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly AppDbContext _context = context;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Products(int? page, string? name = null, bool clear = false, CancellationToken cancellationToken = default)
        {
            ProductsViewSettings settings = HttpContext.Session.Get<ProductsViewSettings>(_productsAdminSettingsSessionKey) ?? new();
            if (clear)
                settings.Filter = new();

            if (!string.IsNullOrEmpty(name))
                settings.Filter.Name = name;

            if (page is not null)
                settings.Pagination.CurrentPage = page.Value;

            settings.Pagination.TotalItems = await _productsService.GetCountAsync(name: settings.Filter.Name, cancellationToken: cancellationToken);
            HttpContext.Session.Set(_productsAdminSettingsSessionKey, settings);
            ViewBag.PageSettings = settings;

            List<LiteProductModel> result = await _productsService.GetAllAsync(
                (settings.Pagination.CurrentPage - 1) * settings.Pagination.ItemsPerPage,
                settings.Pagination.ItemsPerPage,
                null,
                settings.Filter.Name,
                null, 
                null,
                cancellationToken);
            return View(result);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            var genre = _context.Genre.ToList();
            ViewBag.Genre = new SelectList(genre, "Id", "Name");
            CreateProductModel createVinylModel = new();
            return View(createVinylModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductPost(CreateProductModel model, CancellationToken cancellationToken = default)
        {
            await _productsService.AddAsync(model, cancellationToken);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Genres(CancellationToken cancellationToken = default)
        {
            return View(await _context.Genre.Select(g => g.Name).ToListAsync(cancellationToken));
        }

        [HttpGet]
        public IActionResult CreateGenre()
        {
            CreateGenreModel createGenreModel = new CreateGenreModel();
            return View(createGenreModel);
        }

        [HttpPost]
        public IActionResult CreateGenrePost(CreateGenreModel model)
        {
            GenreEntity genre = new()
            {
                Name = model.Name,
            };
            _context.Genre.Add(genre);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Orders(int? page, CancellationToken cancellationToken = default)
        {
            OrdersViewSettings settings = HttpContext.Session.Get<OrdersViewSettings>(_ordersAdminSettingsSessionKey) ?? new();
            if (page is not null)
                settings.Pagination.CurrentPage = page.Value;

            settings.Pagination.TotalItems = await _ordersService.GetCountAsync(cancellationToken: cancellationToken);
            HttpContext.Session.Set(_ordersAdminSettingsSessionKey, settings);
            ViewBag.PageSettings = settings;
            List<OrderModel> model = await _ordersService.GetAllAsync((settings.Pagination.CurrentPage - 1) * settings.Pagination.ItemsPerPage, settings.Pagination.ItemsPerPage, cancellationToken);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Users(int? page, CancellationToken cancellationToken = default)
        {
            UsersViewSettings settings = HttpContext.Session.Get<UsersViewSettings>(_usersAdminSettingsSessionKey) ?? new();
            if (page is not null)
                settings.Pagination.CurrentPage = page.Value;

            settings.Pagination.TotalItems = await _userManager.Users.CountAsync(cancellationToken: cancellationToken);
            HttpContext.Session.Set(_usersAdminSettingsSessionKey, settings);
            ViewBag.PageSettings = settings;
            var model = await _userManager.Users
                .Skip((settings.Pagination.CurrentPage - 1) * settings.Pagination.ItemsPerPage)
                .Take(settings.Pagination.ItemsPerPage)
                .ToListAsync(cancellationToken: cancellationToken);

            return View(model);
        }

        [HttpGet("[controller]/users/{id}")]
        public async Task<IActionResult> UserProfile(Guid id, CancellationToken cancellationToken = default)
        {
            UserEntity? user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return NotFound();
            ViewBag.Orders = await _ordersService.GetAllByUserIdAsync(id, cancellationToken);
            return View(user);
        }

        [HttpGet("[controller]/orders/{id}")]
        public async Task<IActionResult> OpenOrder(Guid id, CancellationToken cancellationToken = default)
        {
            OrderModel? order = await _ordersService.GetByIdAsync(id, cancellationToken);
            if (order is null) return NotFound();

            ViewBag.OrderStatuses = EnumHelper.EnumToSelectList<OrderStatus>();
            return View(order);
        }

        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, OrderStatus status, CancellationToken cancellationToken = default)
        {
            await _ordersService.UpdateAsync(orderId, status, cancellationToken);
            return RedirectToAction("OpenOrder", new { id = orderId });
        }
    }
}
