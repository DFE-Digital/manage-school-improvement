using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService,ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService,errorService),IDateValidationMessageProvider
{
    [BindProperty(Name = "name")]
    [NameValidation]
    public string? Name  { get; set; }
    
    [EmailValidation(ErrorMessage = "Email address must be in correct format")]
    [BindProperty(Name = "email-address")]
    
    public string? EmailAddress  { get; set; }
    
    public bool ShowError { get; set; }
    
    [BindProperty(Name = "date-supporting-organisation-details-added", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    
    public DateTime? DateSupportingOrganisationDetailsAdded  { get; set; }
    
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the date the supporting organisation contact details were added";
    }
    
    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        Name = SupportProject.SupportingOrganisationContactName;
        EmailAddress = SupportProject.SupportingOrganisationContactEmailAddress;
        DateSupportingOrganisationDetailsAdded = SupportProject.DateSupportingOrganisationContactDetailsAdded;

        return Page();
    }
    
    public async Task<IActionResult> OnPost(int id,CancellationToken cancellationToken)
    {

        if (EmailAddress != null && EmailAddress.Any(char.IsWhiteSpace))
        {
            ModelState.AddModelError("email-address", "Email address must not contain spaces");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        var request = new SetSupportingOrganisationContactDetailsCommand(new SupportProjectId(id),Name,EmailAddress,DateSupportingOrganisationDetailsAdded );

        var result = await mediator.Send(request, cancellationToken);
       
        if (result != true)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        TempData["taskUpdated"] = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }

}
