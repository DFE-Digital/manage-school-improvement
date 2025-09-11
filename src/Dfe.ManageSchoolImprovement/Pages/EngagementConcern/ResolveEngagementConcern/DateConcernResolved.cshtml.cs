using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernResolvedDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.ResolveEngagementConcern;

public class DateConcernResolvedModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
    IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "date-concern-resolved", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Enter date concern was resolved")]
    public DateTime? DateConcernResolved { get; set; }

    public DateInputViewModel DateInputViewModel { get; set; } = new();

    public bool ShowError => _errorService.HasErrors();

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return "You must enter a date";
    }

    string IDateValidationMessageProvider.DefaultMessage => "You must enter a valid date";

    (bool, string) IDateValidationMessageProvider.ContextSpecificValidation(int day, int month, int year)
    {
        return (true, string.Empty);
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.ResolveEngagementConcern.Page;

        await base.GetSupportProject(id, cancellationToken);

        // Set up the date input view model
        DateInputViewModel = new DateInputViewModel
        {
            Id = "date-concern-resolved",
            Name = "date-concern-resolved",
            Label = "Enter date concern was resolved",
            HeadingLabel = true,
            Hint = "For example, 1 7 2024.",
            Day = "",
            Month = "",
            Year = ""
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Rebuild the date input view model for display
        DateInputViewModel = new DateInputViewModel
        {
            Id = "date-concern-resolved",
            Name = "date-concern-resolved",
            Label = "Enter date concern was resolved",
            HeadingLabel = true,
            Hint = "For example, 1 7 2024.",
            Day = Request.Form["date-concern-resolved-day"].FirstOrDefault() ?? "",
            Month = Request.Form["date-concern-resolved-month"].FirstOrDefault() ?? "",
            Year = Request.Form["date-concern-resolved-year"].FirstOrDefault() ?? ""
        };

        if (!ModelState.IsValid)
        {
            // Set error state for the date input
            var dayError = ModelState.ContainsKey("date-concern-resolved-day") && ModelState["date-concern-resolved-day"]?.Errors.Count > 0;
            var monthError = ModelState.ContainsKey("date-concern-resolved-month") && ModelState["date-concern-resolved-month"]?.Errors.Count > 0;
            var yearError = ModelState.ContainsKey("date-concern-resolved-year") && ModelState["date-concern-resolved-year"]?.Errors.Count > 0;

            DateInputViewModel.DayInvalid = dayError;
            DateInputViewModel.MonthInvalid = monthError;
            DateInputViewModel.YearInvalid = yearError;
            DateInputViewModel.ErrorMessage = "You must enter a date";

            return Page();
        }

        var request = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                            new SupportProjectId(id),
                            SupportProject?.EngagementConcernResolved,
                            SupportProject?.EngagementConcernResolvedDetails,
                            DateConcernResolved);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        // For now, redirect back to the resolve concern index
        return RedirectToPage(@Links.EngagementConcern.ResolveEngagementConcern.Page, new { id });
    }
}
