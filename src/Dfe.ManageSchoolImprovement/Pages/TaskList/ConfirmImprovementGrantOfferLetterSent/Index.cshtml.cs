using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetConfirmImprovementGrantOfferLetterDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ConfirmImprovementGrantOfferLetterSent;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "date-improvement-grant-offer-letter-sent", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date improvement grant offer letter sent")]
    public DateTime? DateImprovementGrantOfferLetterSent { get; set; }

    public bool ShowError { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        DateImprovementGrantOfferLetterSent = SupportProject.DateImprovementGrantOfferLetterSent;

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

        var request = new SetConfirmImprovementGrantOfferLetterDetailsCommand(new SupportProjectId(id), DateImprovementGrantOfferLetterSent);

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
