using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VinylShop.Web.Extensions;
using VinylShop.Web.Models.Orders;
using VinylShop.Web.Services;

namespace VinylShop.Web.Controllers
{
    [Authorize]
    public class OrdersController(IOrdersService ordersService) : Controller
    {
        private readonly IOrdersService _ordersService = ordersService;

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            Guid? userId = User.Id();
            List<OrderModel> orders = await _ordersService.GetAllByUserIdAsync(userId.Value, cancellationToken);
            return View(orders);
        }

        public async Task<IActionResult> CreateOrder(CancellationToken cancellationToken = default)
        {
            Guid? userId = User.Id();
            OrderModel order = await _ordersService.CreateAsync(userId.Value, cancellationToken);
            return View("OrderConfirmation", order);
        }
    }
}
