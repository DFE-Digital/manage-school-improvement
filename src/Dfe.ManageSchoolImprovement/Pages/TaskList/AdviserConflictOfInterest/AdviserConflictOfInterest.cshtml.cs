using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.
    SetAdviserConflictOfInterestDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AdviserConflictOfInterest;

public class AdviserConflictOfInterest(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
    IDateValidationMessageProvider
{
    [BindProperty(Name = "review-advisers-conflict-of-interest-form")]
    public bool? ReviewAdvisersConflictOfInterestForm { get; set; }

    [BindProperty(Name = "date-conflict-of-interest-declaration-checked", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date conflict of interest declaration checked")]
    public DateTime? DateConflictOfInterestDeclarationChecked { get; set; }

    public bool ShowError { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        ReviewAdvisersConflictOfInterestForm = SupportProject.ReviewAdvisersConflictOfInterestForm;

        DateConflictOfInterestDeclarationChecked = SupportProject.DateConflictOfInterestDeclarationChecked;

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

        var request = new SetAdviserConflictOfInterestDetailsCommand(new SupportProjectId(id),
            ReviewAdvisersConflictOfInterestForm, DateConflictOfInterestDeclarationChecked);

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