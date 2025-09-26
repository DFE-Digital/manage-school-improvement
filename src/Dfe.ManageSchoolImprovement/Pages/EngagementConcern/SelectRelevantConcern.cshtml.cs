using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class SelectRelevantConcernModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty(Name = "SelectedConcernId")]
    public int? SelectedConcernId { get; set; }

    public List<ConcernSummary> AvailableConcerns { get; set; } = new();

    public bool ShowError => _errorService.HasErrors();

    private const string ConcernSelectionKey = "SelectedConcernId";

    public bool ShowConcernSelectionError => ModelState.ContainsKey(ConcernSelectionKey) &&
                                             ModelState[ConcernSelectionKey]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, string? returnPage, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = returnPage ?? Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        // Populate available concerns - this would typically come from a service
        // For now, using sample data as shown in the image
        AvailableConcerns = new List<ConcernSummary>
        {
            new ConcernSummary
            {
                Id = 1,
                Summary = "summary 1",
                RecordedDate = new DateTime(2025, 3, 1)
            },
            new ConcernSummary
            {
                Id = 2,
                Summary = "summary 2",
                RecordedDate = new DateTime(2025, 11, 12)
            }
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (!SelectedConcernId.HasValue)
        {
            ModelState.AddModelError(ConcernSelectionKey, "You must select a concern");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            
            // Re-populate available concerns for display
            AvailableConcerns = new List<ConcernSummary>
            {
                new ConcernSummary
                {
                    Id = 1,
                    Summary = "summary 1",
                    RecordedDate = new DateTime(2025, 3, 1)
                },
                new ConcernSummary
                {
                    Id = 2,
                    Summary = "summary 2",
                    RecordedDate = new DateTime(2025, 11, 12)
                }
            };
            
            return Page();
        }

        // Here you would typically process the selected concern
        // For example, redirect to a specific concern detail page or perform an action
        
        return RedirectToPage(ReturnPage, new { id });
    }
}

public class ConcernSummary
{
    public int Id { get; set; }
    public string Summary { get; set; } = string.Empty;
    public DateTime RecordedDate { get; set; }
}
