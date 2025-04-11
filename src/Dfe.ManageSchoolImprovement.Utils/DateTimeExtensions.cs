using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Dfe.ManageSchoolImprovement.Utils
{
    public static class DateTimeExtensions
    {
        public static string UtcDateTimeToGdsDateTimeString(this DateTime utcDateTime)
        {

            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }

            string timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "GMT Standard Time"
                : "Europe/London";

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
            var formatted = dateTime.ToString("dd MMMM yyyy 'at' h:mmtt", CultureInfo.InvariantCulture);
            formatted = Regex.Replace(formatted, "AM|PM", m => m.Value.ToLower());

            return formatted;
        }

    }
}
