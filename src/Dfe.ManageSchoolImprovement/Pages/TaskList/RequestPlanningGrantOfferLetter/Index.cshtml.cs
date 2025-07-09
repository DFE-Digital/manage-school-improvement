using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetRequestPlanningGrantOfferLetterDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RequestPlanningGrantOfferLetter;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator, IConfiguration configuration) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "date-grant-team-contacted", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date grant team contacted")]
    public DateTime? DateGrantTeamContacted { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    
    [BindProperty(Name = "include-contact-details")]
    public bool? IncludeContactDetails { get; set; }
    
    [BindProperty(Name = "confirm-amount-funding")]
    public bool? ConfirmAmountOfFunding { get; set; }
    
    [BindProperty(Name = "copy-regional-director")]
    public bool? CopyInRegionalDirector { get; set; }
    
    [BindProperty(Name = "email-rise-grant-team")]
    public bool? EmailRiseGrantTeam { get; set; }

    public bool ShowError { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the date grant team contacted";
    }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        DateGrantTeamContacted = SupportProject.DateTeamContactedForRequestingPlanningGrantOfferLetter;
        IncludeContactDetails = SupportProject.IncludeContactDetailsRequestingPlanningGrantOfferEmail;
        ConfirmAmountOfFunding = SupportProject.ConfirmAmountOfPlanningGrantFundingRequested;
        CopyInRegionalDirector = SupportProject.CopyInRegionalDirectorRequestingPlanningGrantOfferEmail;
        EmailRiseGrantTeam = SupportProject.SendRequestingPlanningGrantOfferEmailToRiseGrantTeam;
        
        EmailAddress = configuration.GetValue<string>("EmailForGrantOfferLetter") ?? string.Empty;
        
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

        var request = new SetRequestPlanningGrantOfferLetterDetailsCommand(new SupportProjectId(id), DateGrantTeamContacted, IncludeContactDetails, ConfirmAmountOfFunding, CopyInRegionalDirector, EmailRiseGrantTeam);

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
