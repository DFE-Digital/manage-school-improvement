using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Extensions;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Validation;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;

public class IndexModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ITrustsClient trustClient,
    IEstablishmentsClient establishmentsClient,
    ErrorService errorService,
    IMediator mediator,
    IDateTimeProvider dateTimeProvider) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
    IDateValidationMessageProvider
{
    [BindProperty(Name = "name")]
    [NameValidation]
    public string? Name { get; set; }

    [EmailValidation(ErrorMessage = "Email address must be in correct format")]
    [BindProperty(Name = "email-address")]
    public string? EmailAddress { get; set; }

    [PhoneValidation]
    [BindProperty(Name = "phone-number")]
    public string? PhoneNumber { get; set; }

    [BindProperty] public string? SupportingOrganisationName { get; set; }
    
    [BindProperty] public string? SupportingOrganisationId { get; set; }
    
    [BindProperty] public string? SupportingOrganisationSchoolType { get; set; }
    
    [BindProperty] public bool SupportingSchoolIsAcademy { get; set; }
    
    public bool TaskIsComplete { get; set; }
    
    public TaskListStatus? TaskListStatus { get; set; }
    public ProjectStatusValue? ProjectStatus { get; set; }

    public bool ShowError { get; set; }

    public ContactViewModel? AccountingOfficer { get; set; } = new()
    {
        Name = "",
        Email = "",
        Phone = "",
        Address = "",
    };

    public const string AccountingOfficerRole = "Accounting Officer";
    public const string HeadteacherRole = "Head Teacher";
    
    private const string OrganisationTypeSchool = "School";
    private const string OrganisationTypeTrust = "Trust";
    
    public async Task<IActionResult> OnGet(int id, bool address, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        Name = SupportProject?.SupportingOrganisationContactName;
        EmailAddress = SupportProject?.SupportingOrganisationContactEmailAddress;
        PhoneNumber = SupportProject?.SupportingOrganisationContactPhone;
        SupportingOrganisationName = SupportProject?.SupportOrganisationName;
        SupportingOrganisationId = SupportProject?.SupportOrganisationIdNumber;
        
        if (SupportProject?.SupportOrganisationIdNumber != null)
        {
            var supportingSchoolTrust = SupportProject.SupportOrganisationType == OrganisationTypeSchool ?
                await getEstablishment.GetEstablishmentTrust(SupportProject.SupportOrganisationIdNumber) : null;
            SupportingSchoolIsAcademy = supportingSchoolTrust != null;
        
            TaskIsComplete = !string.IsNullOrWhiteSpace(Name) 
                             && !string.IsNullOrWhiteSpace(EmailAddress)
                             && !string.IsNullOrWhiteSpace(SupportProject?.SupportingOrganisationContactAddress)
                             && !address;

            if (SupportProject is { SupportOrganisationType: OrganisationTypeSchool, SupportOrganisationIdNumber: not null })
            {
                SupportingOrganisationSchoolType = SupportingSchoolIsAcademy ? "Local authority" : "Academy";
            }

            var preFillFields = Name == null && EmailAddress == null && SupportingOrganisationId != null;

            if (preFillFields)
            {
                if (SupportProject?.SupportOrganisationType == OrganisationTypeTrust)
                {
                    await GetTrustAccountingOfficer(SupportProject.SupportOrganisationIdNumber!, cancellationToken);
                }

                if (SupportProject?.SupportOrganisationType == OrganisationTypeSchool)
                {
                
                    if (SupportingSchoolIsAcademy && supportingSchoolTrust != null)
                    {
                        await GetTrustAccountingOfficer(supportingSchoolTrust.Ukprn, cancellationToken);
                    }
                    else
                    {
                        await GetSchoolAccountingOfficer(SupportProject.SupportOrganisationIdNumber!, cancellationToken);
                    }
                }

                Name = AccountingOfficer?.Name;
                EmailAddress = AccountingOfficer?.Email;
                PhoneNumber = AccountingOfficer?.Phone;
            }
        }
        
        TaskListStatus = TaskStatusViewModel.SupportingOrganisationContactDetailsTaskListStatus(SupportProject);
        ProjectStatus = SupportProject.ProjectStatus;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        Name = Name?.Trim();
        EmailAddress = EmailAddress?.Trim();
        PhoneNumber = PhoneNumber?.Trim();

        if (EmailAddress != null && EmailAddress.Any(char.IsWhiteSpace))
        {
            ModelState.AddModelError("email-address", "Email address must not contain spaces");
        }

        if (EmailAddress == null)
        {
            ModelState.AddModelError("email-address", "Enter an email address");
        }

        if (Name == null)
        {
            ModelState.AddModelError("name", "Enter a name");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        var request = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            SupportProject?.SupportOrganisationName,
            SupportProject?.SupportOrganisationIdNumber,
            SupportProject?.SupportOrganisationType,
            SupportProject?.DateSupportOrganisationChosen,
            SupportProject?.AssessmentToolTwoCompleted,
            SupportProject?.SupportingOrganisationAddress,
            Name,
            EmailAddress,
            PhoneNumber,
            SupportProject?.SupportingOrganisationContactAddress);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        
        return RedirectToPage(@Links.TaskList.ConfirmSupportingOrganisationAddressDetails.Page, new { id });
    }

    private async Task GetTrustAccountingOfficer(string ukprn, CancellationToken cancellationToken)
    {

        var trustContacts = await trustClient
            .GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(ukprn,
                cancellationToken).ConfigureAwait(false);

        if (trustContacts != null && trustContacts.Count > 0)
        {
            var accountingOfficer = trustContacts
                .FirstOrDefault(c => c.Roles.Contains(AccountingOfficerRole) && c.IsCurrent(dateTimeProvider));

            if (accountingOfficer != null)
            {
                AccountingOfficer = new ContactViewModel
                {
                    Name = accountingOfficer.DisplayName,
                    Email = accountingOfficer.Email,
                    Phone = accountingOfficer.Phone,
                    Address = SupportProject?.SupportingOrganisationContactAddress ?? "",
                    RoleName = AccountingOfficerRole
                };
            }
        }
    }

    private async Task GetSchoolAccountingOfficer(string supportingOrganisationId, CancellationToken cancellationToken)
    {
        var supportingSchool =
            await getEstablishment.GetEstablishmentByUrn(supportingOrganisationId);
        var establishmentContacts = await establishmentsClient
            .GetAllPersonsAssociatedWithAcademyByUrnAsync(int.TryParse(supportingOrganisationId, out var urn) ? urn : 0,
                cancellationToken).ConfigureAwait(false);

        var headteacherEmail = string.Empty;

        if (establishmentContacts != null && establishmentContacts.Count > 0)
        {
            // "Head Teacher" does not exist as a governance role type, but the accounting officer should always be the head teacher
            var headteacher = establishmentContacts
                .Where(c => c.Roles.Contains(AccountingOfficerRole) && c.IsCurrent(dateTimeProvider))
                .OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefault();

            if (headteacher != null)
            {
                headteacherEmail = headteacher.Email;
            }
        }

        AccountingOfficer = new ContactViewModel()
        {
            Name = supportingSchool.HeadteacherFirstName + " " + supportingSchool.HeadteacherLastName,
            Email = headteacherEmail,
            Phone = supportingSchool.MainPhone,
            Address = SupportProject?.SupportingOrganisationContactAddress ?? "",
            RoleName = HeadteacherRole
        };
    }
}