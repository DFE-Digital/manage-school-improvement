using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetAdviserDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AssignAdviser;

public class AssignAdviser(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "adviser-email-address")]
    [RiseAdviserEmail]
    public string? AdviserEmailAddress { get; set; }

    [BindProperty(Name = "date-adviser-assigned", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date adviser was assigned")]
    public DateTime? DateAdviserAssigned { get; set; }
    
    public string? Referrer { get; set; }


    public bool ShowError { get; set; }

    public bool AdviserEmailAddressError
    {
        get
        {
            return ModelState.Any(x => x.Key == "adviser-email-address");
        }
    }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the school contacted date";
    }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        AdviserEmailAddress = SupportProject.AdviserEmailAddress;

        DateAdviserAssigned = SupportProject.DateAdviserAssigned;

        Referrer = TempData["AssignmentReferrer"] == null ? @Links.TaskList.Index.Page : TempData["AssignmentReferrer"].ToString();
        
        return Page();
    }

    public async Task<IActionResult> OnPost(int id,string referrer, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            Referrer = referrer;
            return await base.GetSupportProject(id, cancellationToken);
        }

        var request = new SetAdviserDetailsCommand(new SupportProjectId(id), DateAdviserAssigned, AdviserEmailAddress);

        var result = await mediator.Send(request, cancellationToken);

        if (result != true)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken); 
        }
        
        return RedirectToPage(referrer, new { id });
    }

}
