using Newtonsoft.Json;

namespace VinylShop.Web.Models
{
    public class PaginationInfo
    {
        public int TotalItems { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 12;
        public int CurrentPage { get; set; } = 1;
        public int PagesCount => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

    }
}
