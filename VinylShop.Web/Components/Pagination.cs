using Microsoft.AspNetCore.Mvc;
using VinylShop.Web.Models;

namespace VinylShop.Web.Components
{
    [ViewComponent]
    public class Pagination : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationInfo model)
        {
            return View("Pagination", model);
        }
    }
}
