using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernResolvedDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.ResolveEngagementConcern;

public class DateConcernResolvedModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
    IDateValidationMessageProvider
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty(Name = "date-concern-resolved")]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [ModelBinder(BinderType = typeof(DateInputModelBinder))]
    public DateTime? DateConcernResolved { get; set; }

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

        DateConcernResolved = SupportProject?.EngagementConcernResolvedDate;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (!DateConcernResolved.HasValue)
        {
            ModelState.AddModelError("date-concern-resolved", "You must enter a date");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            await base.GetSupportProject(id, cancellationToken);
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
