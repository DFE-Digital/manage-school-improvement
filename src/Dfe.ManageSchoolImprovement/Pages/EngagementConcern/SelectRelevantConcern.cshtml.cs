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

    private const string ConcernSelectionKey = "SelectedConcernId-0";

    public bool ShowConcernSelectionError => ModelState.ContainsKey(ConcernSelectionKey) &&
                                             ModelState[ConcernSelectionKey]?.Errors.Count > 0;
    public bool IsInformationPowers { get; set; } = false;
    public bool ActiveEngagementConcernsWithIeb { get; private set; } = false;
    public bool ActiveEngagementConcernsWithInformationPowers { get; private set; } = false;

    public async Task<IActionResult> OnGetAsync(int id, string? returnPage, string nextPage, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SetAvailableConcerns(nextPage);

        return Page();
    }

    private void SetAvailableConcerns(string? nextPage)
    {
        ActiveEngagementConcernsWithIeb = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated == true).Any() ?? false;
        ActiveEngagementConcernsWithInformationPowers = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InformationPowersInUse == true).Any() ?? false;

        if (nextPage == Links.EngagementConcern.RecordUseOfInterimExecutiveBoard.Page)
        {
            AvailableConcerns = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated != true).OrderBy(x => x.EngagementConcernRaisedDate).ToList();
        }
        else if (nextPage == Links.EngagementConcern.RecordUseOfInformationPowers.Page)
        {
            IsInformationPowers = true;
            AvailableConcerns = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InformationPowersInUse != true).OrderBy(x => x.EngagementConcernRaisedDate).ToList();
        }
    }

    public async Task<IActionResult> OnPostAsync(int id, string? returnPage, string nextPage, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        SetAvailableConcerns(nextPage);

        if (!SelectedConcernId.HasValue)
        {
            ModelState.AddModelError(ConcernSelectionKey, $"Select which concern the {(IsInformationPowers ? "Information powers" : "Interim executive board")} is related to");
        }

        if (!ModelState.IsValid)
        {
            // Create a list that includes both form keys AND our manual error key
            var allKeys = Request.Form.Keys.Union(new[] { ConcernSelectionKey }).ToList();

            _errorService.AddErrors(allKeys, ModelState);

            return Page();
        }

        return RedirectToPage(nextPage, new { id, readableEngagementConcernId = SelectedConcernId });
    }
}
