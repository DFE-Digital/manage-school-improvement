using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ArrangeAdvisersFirstFaceToFaceVisit;

public class ArrangeAdvisersFirstFaceToFaceVisitModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "confirm-adviser-has-note-of-visit-template")]
    public bool? ConfirmAdviserHasNoteOfVisitTemplate { get; set; }

    [BindProperty(Name = "adviser-visit-date", BinderType = typeof(DateInputModelBinder))]
    [Display(Name = "Adviser visit date")]

    public DateTime? AdviserVisitDate { get; set; }

    public bool ShowError { get; set; }
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the adviser visit school date";
    }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {

        await base.GetSupportProject(id, cancellationToken);

        AdviserVisitDate = SupportProject.AdviserVisitDate ?? null;
        ConfirmAdviserHasNoteOfVisitTemplate = SupportProject.GiveTheAdviserTheNoteOfVisitTemplate ?? null;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
    {


        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }

        var request = new SetAdviserVisitDateCommand(new SupportProjectId(id), AdviserVisitDate, ConfirmAdviserHasNoteOfVisitTemplate);

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
