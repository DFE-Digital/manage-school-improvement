#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Dfe.ManageSchoolImprovement.Frontend.Models;

public class ProjectListFilters
{
   public const string FilterTitle = nameof(FilterTitle);
   public const string FilterStatuses = nameof(FilterStatuses);
   public const string FilterOfficers = nameof(FilterOfficers);
   public const string FilterRegions = nameof(FilterRegions);
   public const string FilterLocalAuthorities = nameof(FilterLocalAuthorities);
   public const string FilterAdvisoryBoardDates = nameof(FilterAdvisoryBoardDates);

   private IDictionary<string, object?> _store = null!;
   public List<string> AvailableStatuses { get; set; } = new();
   public List<string> AvailableDeliveryOfficers { get; set; } = new();
   public List<string> AvailableRegions { get; set; } = new();

   public List<string> AvailableLocalAuthorities { get; set; } = new();
   public List<string> AvailableAdvisoryBoardDates { get; set; } = new();

   [BindProperty]
   public string? Title { get; set; }

   [BindProperty]
   public string[] SelectedStatuses { get; set; } = [];

   [BindProperty]
   public string[] SelectedOfficers { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedRegions { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedLocalAuthorities { get; set; } = Array.Empty<string>();

   [BindProperty]
   public string[] SelectedAdvisoryBoardDates { get; set; } = Array.Empty<string>();

   public bool IsVisible => !string.IsNullOrWhiteSpace(Title) ||
                            SelectedStatuses.Length > 0 ||
                            SelectedOfficers.Length > 0 ||
                            SelectedRegions.Length > 0 ||
                              SelectedLocalAuthorities.Length > 0 ||
                              SelectedAdvisoryBoardDates.Length > 0;

   public ProjectListFilters PersistUsing(IDictionary<string, object?> store)
   {
      _store = store;

      Title = Get(FilterTitle).FirstOrDefault()?.Trim();
      SelectedStatuses = Get(FilterStatuses);
      SelectedOfficers = Get(FilterOfficers);
      SelectedRegions = Get(FilterRegions);
      SelectedLocalAuthorities = Get(FilterLocalAuthorities);
      SelectedAdvisoryBoardDates = Get(FilterAdvisoryBoardDates);

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
         SelectedRegions = Array.Empty<string>();
         SelectedLocalAuthorities = Array.Empty<string>();
         SelectedAdvisoryBoardDates = Array.Empty<string>();

         return;
      }

      if (query.ContainsKey("remove"))
      {
         SelectedStatuses = GetAndRemove(FilterStatuses, GetFromQuery(nameof(SelectedStatuses)), true);
         SelectedOfficers = GetAndRemove(FilterOfficers, GetFromQuery(nameof(SelectedOfficers)), true);
         SelectedRegions = GetAndRemove(FilterRegions, GetFromQuery(nameof(SelectedRegions)), true);
         SelectedLocalAuthorities = GetAndRemove(FilterLocalAuthorities, GetFromQuery(nameof(SelectedLocalAuthorities)), true);
         SelectedAdvisoryBoardDates = GetAndRemove(FilterAdvisoryBoardDates, GetFromQuery(nameof(SelectedAdvisoryBoardDates)), true);

         return;
      }

      bool activeFilterChanges = query.ContainsKey(nameof(Title)) ||
                                 query.ContainsKey(nameof(SelectedStatuses)) ||
                                 query.ContainsKey(nameof(SelectedOfficers)) ||
                                 query.ContainsKey(nameof(SelectedRegions)) ||
                                 query.ContainsKey(nameof(SelectedLocalAuthorities)) ||
                                 query.ContainsKey(nameof(SelectedAdvisoryBoardDates));

      if (activeFilterChanges)
      {
         Title = Cache(FilterTitle, GetFromQuery(nameof(Title))).FirstOrDefault()?.Trim();
         SelectedStatuses = Cache(FilterStatuses, GetFromQuery(nameof(SelectedStatuses)));
         SelectedOfficers = Cache(FilterOfficers, GetFromQuery(nameof(SelectedOfficers)));
         SelectedRegions = Cache(FilterRegions, GetFromQuery(nameof(SelectedRegions)));
         SelectedLocalAuthorities = Cache(FilterLocalAuthorities, GetFromQuery(nameof(SelectedLocalAuthorities)));
         SelectedAdvisoryBoardDates = Cache(FilterAdvisoryBoardDates, GetFromQuery(nameof(SelectedAdvisoryBoardDates)));
      }
      else
      {
         Title = Get(FilterTitle, true).FirstOrDefault()?.Trim();
         SelectedStatuses = Get(FilterStatuses, true);
         SelectedOfficers = Get(FilterOfficers, true);
         SelectedRegions = Get(FilterRegions, true);
         SelectedLocalAuthorities = Get(FilterLocalAuthorities, true);
         SelectedAdvisoryBoardDates = Get(FilterAdvisoryBoardDates, true);
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
      Cache(FilterRegions, default);
      Cache(FilterLocalAuthorities, default);
      Cache(FilterAdvisoryBoardDates, default);
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
