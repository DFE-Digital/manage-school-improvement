using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernResolvedDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.ResolveEngagementConcern;

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "resolution-details")]
    public string? ResolutionDetails { get; set; }

    [BindProperty(Name = "mark-concern-resolved")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? MarkConcernResolved { get; set; }

    public bool ShowError => _errorService.HasErrors();

    private const string ResolutionDetailsKey = "resolution-details";

    public bool ShowResolutionDetailsError => ModelState.ContainsKey(ResolutionDetailsKey) && ModelState[ResolutionDetailsKey]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        // Pre-check the checkbox if there's already a resolution recorded
        MarkConcernResolved = SupportProject.EngagementConcernResolved;
        ResolutionDetails = SupportProject.EngagementConcernResolvedDetails;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Validate that if checkbox is checked, details must be provided
        if (MarkConcernResolved == true && string.IsNullOrWhiteSpace(ResolutionDetails))
        {
            ModelState.AddModelError(ResolutionDetailsKey, "You must enter details");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var request = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
            new SupportProjectId(id),
            MarkConcernResolved,
            ResolutionDetails,
            SupportProject.EngagementConcernResolvedDate);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        // For now, redirect back to the engagement concern index
        return RedirectToPage(@Links.EngagementConcern.ResolveEngagementConcernDate.Page, new { id });
    }
}
