#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Dfe.ManageSchoolImprovement.Frontend.Models;

public class ProjectListFilters
{
    public const string FilterTitle = nameof(FilterTitle);
    public const string FilterStatuses = nameof(FilterStatuses);
    public const string FilterOfficers = nameof(FilterOfficers);
    public const string FilterAdvisers = nameof(FilterAdvisers);
    public const string FilterRegions = nameof(FilterRegions);
    public const string FilterLocalAuthorities = nameof(FilterLocalAuthorities);
    public const string FilterTrusts = nameof(FilterTrusts);
    public const string FilterMonths = nameof(FilterMonths);
    public const string FilterYears = nameof(FilterYears);

    private IDictionary<string, object?> _store = null!;
    public List<string> AvailableDeliveryOfficers { get; set; } = new();
    public List<string> AvailableAdvisers { get; set; } = new();
    public List<string> AvailableRegions { get; set; } = new();
    public List<KeyValuePair<string, string>> AvailableStatuses { get; set; } = new();
    public List<string> AvailableTrusts { get; set; } = new();
    public List<string> AvailableLocalAuthorities { get; set; } = new();
    public List<string> AvailableYears { get; set; } = new();

    [BindProperty] public string? Title { get; set; }

    [BindProperty] public string[] SelectedStatuses { get; set; } = [];

    [BindProperty] public string[] SelectedOfficers { get; set; } = Array.Empty<string>();

    [BindProperty] public string[] SelectedAdvisers { get; set; } = Array.Empty<string>();

    [BindProperty] public string[] SelectedRegions { get; set; } = Array.Empty<string>();

    [BindProperty] public string[] SelectedLocalAuthorities { get; set; } = Array.Empty<string>();

    [BindProperty] public string[] SelectedTrusts { get; set; } = Array.Empty<string>();

    [BindProperty] public string[] SelectedYears { get; set; } = Array.Empty<string>();

    [BindProperty] public string[] SelectedMonths { get; set; } = Array.Empty<string>();

    public List<string> YearsChecked { get; set; } = new();

    private static List<string> Months = new()
    {
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    };
    public static List<string> _months => Months;


    private static List<string> MonthsInCurrentYear = _months.Take(DateTime.Now.Month).ToList();
    public static List<string> _monthsInCurrentYear => MonthsInCurrentYear;

    public void RemoveYearsInSelectedMonths(IEnumerable<KeyValuePair<string, StringValues>> requestQuery)
    {
        foreach (var year in SelectedYears)
        {
            var hasMonthsForYear = SelectedMonths.Any(month => month.StartsWith($"{year} "));

            if (hasMonthsForYear)
            {
                SelectedYears = SelectedYears.Where(x => x != year).ToArray();
                YearsChecked.Add(year);
            }

            Dictionary<string, StringValues>? query = new(requestQuery, StringComparer.OrdinalIgnoreCase);

            if (query.ContainsKey("remove") && query.ContainsKey(nameof(SelectedMonths)))
            {
                var monthQuery = query.TryGetValue(nameof(SelectedMonths), out StringValues value) ? value.ToArray() : Array.Empty<string>();

                var hasMonths = monthQuery.Any(month => month != null && month.StartsWith($"{year} "));

                if (hasMonths)
                {
                    SelectedYears = SelectedYears.Where(x => x != year).ToArray();
                    YearsChecked = YearsChecked.Where(x => x != year).ToList();
                }
            }
        }
    }

    public void RemoveMonthsIfYearUnchecked()
    {
        SelectedMonths = SelectedMonths
            .Where(month =>
            {
                var yearFromMonth = month.Split(' ')[0];
                return SelectedYears.Contains(yearFromMonth);
            })
            .ToArray();
    }

    public bool IsVisible => !string.IsNullOrWhiteSpace(Title) ||
                             SelectedStatuses.Length > 0 ||
                             SelectedAdvisers.Length > 0 ||
                             SelectedOfficers.Length > 0 ||
                             SelectedRegions.Length > 0 ||
                             SelectedLocalAuthorities.Length > 0 ||
                             SelectedTrusts.Length > 0 ||
                             SelectedMonths.Length > 0 ||
                             SelectedYears.Length > 0;

    public ProjectListFilters PersistUsing(IDictionary<string, object?> store)
    {
        _store = store;

        Title = Get(FilterTitle).FirstOrDefault()?.Trim();
        SelectedStatuses = Get(FilterStatuses);
        SelectedOfficers = Get(FilterOfficers);
        SelectedAdvisers = Get(FilterAdvisers);
        SelectedRegions = Get(FilterRegions);
        SelectedLocalAuthorities = Get(FilterLocalAuthorities);
        SelectedTrusts = Get(FilterTrusts);
        SelectedMonths = Get(FilterMonths);
        SelectedYears = Get(FilterYears);

        return this;
    }

    public void PopulateFrom(IEnumerable<KeyValuePair<string, StringValues>> requestQuery)
    {
        Dictionary<string, StringValues>? query = new(requestQuery, StringComparer.OrdinalIgnoreCase);

        if (query.ContainsKey("clear"))
        {
            ClearFilters();

            Title = default;
            SelectedStatuses = Array.Empty<string>();
            SelectedOfficers = Array.Empty<string>();
            SelectedAdvisers = Array.Empty<string>();
            SelectedRegions = Array.Empty<string>();
            SelectedLocalAuthorities = Array.Empty<string>();
            SelectedTrusts = Array.Empty<string>();
            SelectedMonths = Array.Empty<string>();
            SelectedYears = Array.Empty<string>();

            return;
        }

        if (query.ContainsKey("remove"))
        {
            SelectedStatuses = GetAndRemove(FilterStatuses, GetFromQuery(nameof(SelectedStatuses)), true);
            SelectedOfficers = GetAndRemove(FilterOfficers, GetFromQuery(nameof(SelectedOfficers)), true);
            SelectedAdvisers = GetAndRemove(FilterAdvisers, GetFromQuery(nameof(SelectedAdvisers)), true);
            SelectedRegions = GetAndRemove(FilterRegions, GetFromQuery(nameof(SelectedRegions)), true);
            SelectedLocalAuthorities =
                GetAndRemove(FilterLocalAuthorities, GetFromQuery(nameof(SelectedLocalAuthorities)), true);
            SelectedTrusts = GetAndRemove(FilterTrusts, GetFromQuery(nameof(SelectedTrusts)), true);
            SelectedMonths = GetAndRemove(FilterMonths, GetFromQuery(nameof(SelectedMonths)), true);
            SelectedYears = GetAndRemove(FilterYears, GetFromQuery(nameof(SelectedYears)), true);

            return;
        }

        bool activeFilterChanges = query.ContainsKey(nameof(Title)) ||
                                   query.ContainsKey(nameof(SelectedStatuses)) ||
                                   query.ContainsKey(nameof(SelectedOfficers)) ||
                                   query.ContainsKey(nameof(SelectedAdvisers)) ||
                                   query.ContainsKey(nameof(SelectedRegions)) ||
                                   query.ContainsKey(nameof(SelectedLocalAuthorities)) ||
                                   query.ContainsKey(nameof(SelectedMonths)) ||
                                   query.ContainsKey(nameof(SelectedYears)) ||
                                   query.ContainsKey(nameof(SelectedTrusts));

        if (activeFilterChanges)
        {
            Title = Cache(FilterTitle, GetFromQuery(nameof(Title))).FirstOrDefault()?.Trim();
            SelectedStatuses = Cache(FilterStatuses, GetFromQuery(nameof(SelectedStatuses)));
            SelectedOfficers = Cache(FilterOfficers, GetFromQuery(nameof(SelectedOfficers)));
            SelectedAdvisers = Cache(FilterAdvisers, GetFromQuery(nameof(SelectedAdvisers)));
            SelectedRegions = Cache(FilterRegions, GetFromQuery(nameof(SelectedRegions)));
            SelectedLocalAuthorities = Cache(FilterLocalAuthorities, GetFromQuery(nameof(SelectedLocalAuthorities)));
            SelectedMonths = Cache(FilterMonths, GetFromQuery(nameof(SelectedMonths)));
            SelectedYears = Cache(FilterYears, GetFromQuery(nameof(SelectedYears)));
            SelectedTrusts = Cache(FilterTrusts, GetFromQuery(nameof(SelectedTrusts)));
        }
        else
        {
            Title = Get(FilterTitle, true).FirstOrDefault()?.Trim();
            SelectedStatuses = Get(FilterStatuses, true);
            SelectedOfficers = Get(FilterOfficers, true);
            SelectedAdvisers = Get(FilterAdvisers, true);
            SelectedRegions = Get(FilterRegions, true);
            SelectedLocalAuthorities = Get(FilterLocalAuthorities, true);
            SelectedMonths = Get(FilterMonths, true);
            SelectedYears = Get(FilterYears, true);
            SelectedTrusts = Get(FilterTrusts, true);
        }

        string[] GetFromQuery(string key)
        {
            return query.ContainsKey(key) ? query[key]! : Array.Empty<string>();
        }
    }

    private string[] Get(string key, bool persist = false)
    {
        if (!_store.TryGetValue(key, out var value1)) return Array.Empty<string>();

        string[]? value = (string[]?)value1;
        if (persist) Cache(key, value);

        return value ?? Array.Empty<string>();
    }

    private string[] GetAndRemove(string key, string[]? value, bool persist = false)
    {
        if (!_store.TryGetValue(key, out var value1)) return Array.Empty<string>();

        string[]? currentValues = (string[]?)value1;

        if (value is not null && value.Length > 0 && currentValues is not null)
        {
            currentValues = currentValues.Where(x => !value.Contains(x)).ToArray();
        }

        if (persist) Cache(key, currentValues);

        return currentValues ?? Array.Empty<string>();
    }

    private string[] Cache(string key, string[]? value)
    {
        if (value is null || value.Length == 0)
            _store.Remove(key);
        else
            _store[key] = value;

        return value ?? Array.Empty<string>();
    }

    private void ClearFilters()
    {
        Cache(FilterTitle, default);
        Cache(FilterStatuses, default);
        Cache(FilterOfficers, default);
        Cache(FilterAdvisers, default);
        Cache(FilterRegions, default);
        Cache(FilterLocalAuthorities, default);
        Cache(FilterMonths, default);
        Cache(FilterTrusts, default);
        Cache(FilterYears, default);
    }

    /// <summary>
    ///    Removes all project list filters from the store
    /// </summary>
    /// <param name="store">the store used to cache filters between pages</param>
    /// <remarks>
    ///    Note that, when using TempData, this won't take effect until after the next request context that returns a 2xx response!
    /// </remarks>
    public static void ClearFiltersFrom(IDictionary<string, object?> store)
    {
        new ProjectListFilters().PersistUsing(store).ClearFilters();
    }
}