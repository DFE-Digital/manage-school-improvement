using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "organisation-name")]
    public string? OrganisationName { get; set; }

    [BindProperty(Name = "id-number")]

    public string? IdNumber { get; set; }

    public bool ShowError { get; set; }

    [BindProperty(Name = "date-support-organisation-chosen", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]

    public DateTime? DateSupportOrganisationChosen { get; set; }

    [BindProperty(Name = "complete-assessment-tool")]
    public bool? CompleteAssessmentTool { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        OrganisationName = SupportProject.SupportOrganisationName;
        IdNumber = SupportProject.SupportOrganisationIdNumber;
        DateSupportOrganisationChosen = SupportProject.DateSupportOrganisationChosen;
        CompleteAssessmentTool = SupportProject.AssessmentToolTwoCompleted;
        return Page();
    }

    public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
    {
        OrganisationName = OrganisationName?.Trim();

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }

        var request = new SetChoosePreferredSupportingOrganisationCommand(new SupportProjectId(id), OrganisationName, IdNumber, DateSupportOrganisationChosen, CompleteAssessmentTool);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }

}
