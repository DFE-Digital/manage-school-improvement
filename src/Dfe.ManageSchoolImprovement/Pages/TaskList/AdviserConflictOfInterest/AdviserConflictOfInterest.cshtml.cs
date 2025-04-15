using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetAdviserConflictOfInterestDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AdviserConflictOfInterest;

public class AdviserConflictOfInterest(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "send-conflict-of-interest-form-to-proposed-adviser-and-the-school")]
    public bool? SendConflictOfInterestFormToProposedAdviserAndTheSchool { get; set; }

    [BindProperty(Name = "receive-completed-conflict-of-interest-form")]
    public bool? ReceiveCompletedConflictOfInterestForm { get; set; }

    [BindProperty(Name = "save-completed-conflict-of-interest-form-in-sharepoint")]
    public bool? SaveCompletedConflictOfinterestFormInSharePoint { get; set; }

    [BindProperty(Name = "date-conflicts-of-interest-were-checked", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date conflicts of interest were checked")]
    public DateTime? DateConflictsOfInterestWereChecked { get; set; }

    public bool ShowError { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the date conflicts of interest were checked";
    }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        SendConflictOfInterestFormToProposedAdviserAndTheSchool = SupportProject.SendConflictOfInterestFormToProposedAdviserAndTheSchool;

        ReceiveCompletedConflictOfInterestForm = SupportProject.ReceiveCompletedConflictOfInterestForm;

        SaveCompletedConflictOfinterestFormInSharePoint = SupportProject.SaveCompletedConflictOfinterestFormInSharePoint;

        DateConflictsOfInterestWereChecked = SupportProject.DateConflictsOfInterestWereChecked;

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

        var request = new SetAdviserConflictOfInterestDetailsCommand(new SupportProjectId(id), SendConflictOfInterestFormToProposedAdviserAndTheSchool, ReceiveCompletedConflictOfInterestForm, SaveCompletedConflictOfinterestFormInSharePoint, DateConflictsOfInterestWereChecked);

        var result = await mediator.Send(request, cancellationToken);

        if (result != true)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken); ;
        }

        TaskUpdated = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }

}
