using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace VinylShop.Web.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> EnumToSelectList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => new SelectListItem
                       {
                           Text = GetDisplayName(e) ?? e.ToString(),
                           Value = Convert.ToInt32(e).ToString()
                       });
        }

        public static string GetDisplayName<T>(T enumValue) where T : Enum
        {
            var displayAttribute = enumValue.GetType()
                                            .GetField(enumValue.ToString())
                                            .GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name;
        }

    }
}
