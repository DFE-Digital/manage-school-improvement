using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectCaseStudyDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.CaseStudy;

public class SetDetailsModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "case-study-details")]
    public string? CaseStudyDetails { get; set; }

    [BindProperty(Name = "case-study-candidate")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? CaseStudyCandidate { get; set; }

    public bool ShowError => _errorService.HasErrors();
    
    public const string CaseStudyDetailsKey = "case-study-details";

    public bool ShowCaseStudyDetailsError => ModelState.ContainsKey(CaseStudyDetailsKey) && ModelState[CaseStudyDetailsKey]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.CaseStudy.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        CaseStudyCandidate = SupportProject.CaseStudyCandidate;
        CaseStudyDetails = SupportProject.CaseStudyDetails;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        // set support project so we can compare values for success banner
        await base.GetSupportProject(id, cancellationToken);

        if (CaseStudyCandidate is true && string.IsNullOrEmpty(CaseStudyDetails))
        {
            _errorService.AddError(CaseStudyDetailsKey, "You must enter details");
            ModelState.AddModelError(CaseStudyDetailsKey, "You must enter details");

            return Page();
        }

        TempData["CaseStudyUpdated"] = SupportProject.CaseStudyCandidate is true && CaseStudyCandidate is true && SupportProject.CaseStudyDetails != CaseStudyDetails;
        TempData["CaseStudyAdded"] = (SupportProject.CaseStudyCandidate is null || SupportProject.CaseStudyCandidate is false) && CaseStudyCandidate is true;
        TempData["CaseStudyRemoved"] = CaseStudyCandidate is false;
        //reset details if removed
        if (CaseStudyCandidate is false)
        {
            CaseStudyDetails = null;
        }
        var request = new SetSupportProjectCaseStudyDetailsCommand(new SupportProjectId(id), CaseStudyCandidate, CaseStudyDetails);

        var result = await mediator.Send(request, cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        return RedirectToPage(@Links.CaseStudy.Index.Page, new { id });
    }
}
