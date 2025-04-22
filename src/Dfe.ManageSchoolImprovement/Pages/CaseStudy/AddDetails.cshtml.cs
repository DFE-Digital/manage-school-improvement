using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectCaseStudyDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.CaseStudy;

public class AddDetailsModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "case-study-details")]
    public string? CaseStudyDetails { get; set; }

    [BindProperty(Name = "case-study-candidate")]
    public bool? CaseStudyCandidate { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.Notes.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(CaseStudyDetails))
        {
            _errorService.AddError("case-study-details", "Enter details");

            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        var request = new SetSupportProjectCaseStudyDetailsCommand(new SupportProjectId(id), CaseStudyCandidate, CaseStudyDetails);

        var result = await mediator.Send(request, cancellationToken);

        if (result == null)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TempData["newNote"] = true;

        return RedirectToPage(@Links.Notes.Index.Page, new { id });
    }
}
