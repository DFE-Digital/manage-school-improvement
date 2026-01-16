using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "name")]
    [NameValidation]
    public string? Name { get; set; }

    [EmailValidation(ErrorMessage = "Email address must be in correct format")]
    [BindProperty(Name = "email-address")]
    public string? EmailAddress { get; set; }
    
    [PhoneValidation]
    [BindProperty(Name= "phone-number")]
    public string? PhoneNumber { get; set; }

    public bool ShowError { get; set; }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        Name = SupportProject?.SupportingOrganisationContactName;
        EmailAddress = SupportProject?.SupportingOrganisationContactEmailAddress;
        PhoneNumber = SupportProject?.SupportingOrganisationContactPhone;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
    {
        // trim any trailing whitespace from the name and email address
        Name = Name?.Trim();
        EmailAddress = EmailAddress?.Trim();
        PhoneNumber = PhoneNumber?.Trim();

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

        var request = new SetSupportingOrganisationContactDetailsCommand(new SupportProjectId(id), Name, EmailAddress, PhoneNumber);

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
