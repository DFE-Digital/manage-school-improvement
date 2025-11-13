namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class DateRangeValidationService
{
    public enum DateRange
    {
        Past,
        PastOrToday,
        Future,
        FutureOrToday,
        PastOrFuture
    }

    public static (bool, string) Validate(DateTime date, DateRange dateRange, string displayName)
    {
        switch (dateRange)
        {
            case DateRange.Past:
                if (date >= DateTime.Today)
                {
                    return (false, $"Enter a date in the past");
                }

                break;

            case DateRange.PastOrToday:
                if (date > DateTime.Today)
                {
                    return (false, "Enter today's date or a date in the past");
                }

                break;

            case DateRange.Future:
                if (date <= DateTime.Today)
                {
                    return (false, "Enter a date in the future");
                }

                break;

            case DateRange.FutureOrToday:
                if (date < DateTime.Today)
                {
                    return (false, "Enter today's date or a date in the future");
                }

                break;
            case DateRange.PastOrFuture:
                return (true, "");
        }

        return (true, "");
    }
}
