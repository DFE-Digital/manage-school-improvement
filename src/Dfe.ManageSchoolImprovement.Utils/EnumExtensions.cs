using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Dfe.ManageSchoolImprovement.Utils
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name ?? value.ToString();
        }

        public static string GetDisplayShortName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetShortName() ?? enumValue.ToString();
        }
    }
}
