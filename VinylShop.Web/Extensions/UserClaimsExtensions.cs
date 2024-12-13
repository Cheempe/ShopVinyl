using System.Security.Claims;

namespace VinylShop.Web.Extensions
{
    public static class UserClaimsExtensions
    {
        public static Guid? Id(this ClaimsPrincipal principal)
        {
            string? stringId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            _ = Guid.TryParse(stringId, out Guid id);
            return id;
        }
    }
}
