using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
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

    public List<EngagementConcernViewModel>? AvailableConcerns { get; set; } = null;

    public bool ShowError => _errorService.HasErrors();

    private const string ConcernSelectionKey = "SelectedConcernId";

    public bool ShowConcernSelectionError => ModelState.ContainsKey(ConcernSelectionKey) &&
                                             ModelState[ConcernSelectionKey]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, string? returnPage, string nextPage, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = returnPage ?? Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SetAvailableConcerns(nextPage);

        return Page();
    }

    private void SetAvailableConcerns(string? nextPage)
    {
        if (nextPage == Links.EngagementConcern.RecordUseOfInterimExecutiveBoard.Page)
        {
            AvailableConcerns = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated != true).OrderBy(x => x.EngagementConcernRaisedDate).ToList();
        }
        else if (nextPage == Links.EngagementConcern.RecordUseOfInformationPowers.Page)
        {
            AvailableConcerns = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InformationPowersInUse != true).OrderBy(x => x.EngagementConcernRaisedDate).ToList();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id, string nextPage, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (!SelectedConcernId.HasValue)
        {
            ModelState.AddModelError(ConcernSelectionKey, "You must select a concern");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);

            SetAvailableConcerns(nextPage);

            return Page();
        }

        return RedirectToPage(nextPage, new { id, readableEngagementConcernId = SelectedConcernId });
    }
}
