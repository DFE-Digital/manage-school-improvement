using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using System.Globalization;

namespace Dfe.ManageSchoolImprovement.Extensions;

public static class PersonsApiExtensions
{
    /// <summary>
    /// Determines if a governance position is historical (i.e., held in the past)
    /// based on comparing the DateTermOfOfficeEnds with today's date.
    /// </summary>
    /// <param name="academyGovernance">The academy governance object</param>
    /// <param name="dateTimeProvider">The date time provider for getting current date</param>
    /// <returns>True if the position is historical (ended), false if it's current or has no end date</returns>
    public static bool IsHistorical(this AcademyGovernance academyGovernance, IDateTimeProvider dateTimeProvider)
    {
        if (academyGovernance?.DateTermOfOfficeEndsEnded == null ||
            string.IsNullOrWhiteSpace(academyGovernance.DateTermOfOfficeEndsEnded))
        {
            return false; // No end date means it's still current
        }

        if (!TryParseDate(academyGovernance.DateTermOfOfficeEndsEnded, out DateTime endDate))
        {
            return false; // If we can't parse the date, assume it's current
        }

        return endDate.Date < dateTimeProvider.Now.Date;
    }

    /// <summary>
    /// Determines if a trust governance position is historical (i.e., held in the past)
    /// based on comparing the DateTermOfOfficeEnds with today's date.
    /// </summary>
    /// <param name="trustGovernance">The trust governance object</param>
    /// <param name="dateTimeProvider">The date time provider for getting current date</param>
    /// <returns>True if the position is historical (ended), false if it's current or has no end date</returns>
    public static bool IsHistorical(this TrustGovernance trustGovernance, IDateTimeProvider dateTimeProvider)
    {
        if (trustGovernance?.DateTermOfOfficeEndsEnded == null ||
            string.IsNullOrWhiteSpace(trustGovernance.DateTermOfOfficeEndsEnded))
        {
            return false; // No end date means it's still current
        }

        if (!TryParseDate(trustGovernance.DateTermOfOfficeEndsEnded, out DateTime endDate))
        {
            return false; // If we can't parse the date, assume it's current
        }

        return endDate.Date < dateTimeProvider.Now.Date;
    }

    /// <summary>
    /// Determines if a governance position is current (i.e., still active)
    /// based on comparing the DateTermOfOfficeEnds with today's date.
    /// </summary>
    /// <param name="academyGovernance">The academy governance object</param>
    /// <param name="dateTimeProvider">The date time provider for getting current date</param>
    /// <returns>True if the position is current, false if it has ended</returns>
    public static bool IsCurrent(this AcademyGovernance academyGovernance, IDateTimeProvider dateTimeProvider)
    {
        return !academyGovernance.IsHistorical(dateTimeProvider);
    }

    /// <summary>
    /// Determines if a trust governance position is current (i.e., still active)
    /// based on comparing the DateTermOfOfficeEnds with today's date.  
    /// </summary>
    /// <param name="trustGovernance">The trust governance object</param>
    /// <param name="dateTimeProvider">The date time provider for getting current date</param>
    /// <returns>True if the position is current, false if it has ended</returns>
    public static bool IsCurrent(this TrustGovernance trustGovernance, IDateTimeProvider dateTimeProvider)
    {
        return !trustGovernance.IsHistorical(dateTimeProvider);
    }

    /// <summary>
    /// Tries to parse a date string using multiple common date formats.
    /// </summary>
    /// <param name="dateString">The date string to parse</param>
    /// <param name="parsedDate">The parsed DateTime if successful</param>
    /// <returns>True if parsing was successful, false otherwise</returns>
    private static bool TryParseDate(string dateString, out DateTime parsedDate)
    {
        parsedDate = default;

        if (string.IsNullOrWhiteSpace(dateString))
            return false;

        // Common date formats that might be used in the PersonsApi
        string[] formats = {
            "yyyy-MM-dd",           // ISO 8601
            "dd/MM/yyyy",           // UK format
            "MM/dd/yyyy",           // US format
            "dd-MM-yyyy",           // UK with dashes
            "MM-dd-yyyy",           // US with dashes
            "yyyy/MM/dd",           // ISO with slashes
            "dd MMM yyyy",          // e.g., "01 Jan 2023"
            "MMM dd, yyyy",         // e.g., "Jan 01, 2023"
            "yyyy-MM-ddTHH:mm:ss",  // ISO 8601 with time
            "yyyy-MM-ddTHH:mm:ssZ", // ISO 8601 with UTC time
            "O",                    // Round-trip format
            "s"                     // Sortable format
        };

        foreach (string format in formats)
        {
            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return true;
            }
        }

        // Fallback to general parsing
        return DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
    }
}