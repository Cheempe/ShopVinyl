using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VinylShop.Web.Data;
using VinylShop.Web.Models;
using Microsoft.AspNetCore.Identity;
using VinylShop.Web.Data.Entities;
using VinylShop.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using VinylShop.Web.Services;

namespace VinylShop.Web.Controllers
{
    [Authorize]
    public class AccountController(UserManager<UserEntity> userManager, IOrdersService ordersService) : Controller
    {
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly IOrdersService _ordersService = ordersService;

        [HttpGet]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken = default)
        {
            Guid id = User.Id()!.Value;
            UserEntity? user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null) return NotFound();
            ViewBag.Orders = await _ordersService.GetAllByUserIdAsync(id, cancellationToken);
            return View(user);
        }

    }

}
