using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Extensions;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Trusts;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class ConfirmSupportingOrganisationDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    ITrustsClient trustClient,
    IGetTrust getTrust,
    IGetEstablishment getEstablishment,
    IEstablishmentsClient establishmentsClient,
    IDateTimeProvider dateTimeProvider,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "date-support-organisation-confirmed", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationConfirmed { get; set; }

    public string? OrganisationAddress { get; set; }
    public string? ContactAddress { get; set; }
    
    public ContactViewModel? AccountingOfficer { get; set; }

    public const string AccountingOfficerRole = "Accounting Officer";
    public const string HeadteacherRole = "Head Teacher";
    public bool ShowError { get; set; }

    public string? DateConfirmedErrorMessage { get; private set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public string PreviousPage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, string? previousPage,
        CancellationToken cancellationToken = default)
    {
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        await base.GetSupportProject(id, cancellationToken);
        
        DateSupportOrganisationConfirmed = SupportProject?.DateSupportOrganisationChosen;
        OrganisationAddress = SupportProject?.SupportingOrganisationAddress;

        if (!string.IsNullOrEmpty(SupportProject?.SupportOrganisationName))
        {
            if (SupportProject.SupportOrganisationType == "Trust")
            {
                await GetTrustAccountingOfficer(SupportProject.SupportOrganisationIdNumber, cancellationToken);
                AccountingOfficer.Address = OrganisationAddress ?? "";
            }

            if (SupportProject?.SupportOrganisationType == "School")
            {
                var expectedSchool = await getEstablishment.GetEstablishmentByUrn(SupportProject.SupportOrganisationIdNumber);

                var expectedTrust = await getEstablishment.GetEstablishmentTrust(expectedSchool.Urn) ?? null;
            
                if (expectedTrust != null)
                {
                    await GetTrustContactAddress(expectedTrust, cancellationToken);
                    await GetTrustAccountingOfficer(expectedTrust.Ukprn, cancellationToken);
                }
                else
                {
                    await GetSchoolAccountingOfficer(SupportProject.SupportOrganisationIdNumber, cancellationToken);
                }
            }  
        }


        return Page();
    }

    public async Task<IActionResult> OnPost(int id, string? previousPage, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        if (SupportProject?.SupportOrganisationType == "Trust")
        {
            await GetTrustAccountingOfficer(SupportProject.SupportOrganisationIdNumber, cancellationToken);
            AccountingOfficer.Address = OrganisationAddress ?? "";
        }

        if (SupportProject?.SupportOrganisationType == "School")
        {
            var expectedSchool = await getEstablishment.GetEstablishmentByUrn(SupportProject?.SupportOrganisationIdNumber);

            var expectedTrust = await getEstablishment.GetEstablishmentTrust(expectedSchool.Urn) ?? null;

            if (expectedTrust != null &&
                !string.IsNullOrEmpty(SupportProject.SupportOrganisationName))
            {
                await GetTrustContactAddress(expectedTrust, cancellationToken);
                await GetTrustAccountingOfficer(expectedTrust.Ukprn, cancellationToken);
            }
            else if (!string.IsNullOrEmpty(SupportProject?.SupportOrganisationName))
            {
                ContactAddress = SupportProject.SupportingOrganisationAddress;
                await GetSchoolAccountingOfficer(SupportProject.SupportOrganisationIdNumber, cancellationToken);
            }
        }
        
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        if (DateSupportOrganisationConfirmed == null)
        {
            DateConfirmedErrorMessage = "Enter a date";
            ModelState.AddModelError("date-support-organisation-confirmed", DateConfirmedErrorMessage);
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await GetSupportProject(id, cancellationToken);
        }

        var dateContactDetailsAdded = DateTime.UtcNow;

        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            SupportProject?.SupportOrganisationName,
            SupportProject?.SupportOrganisationIdNumber,
            SupportProject?.SupportOrganisationType,
            DateSupportOrganisationConfirmed,
            SupportProject?.AssessmentToolTwoCompleted,
            OrganisationAddress,
            AccountingOfficer?.Name,
            AccountingOfficer?.Email,
            AccountingOfficer?.Phone,
            AccountingOfficer?.Address,
            dateContactDetailsAdded);

        var result = await mediator.Send(command, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
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
                    Address = ContactAddress ?? "",
                    RoleName = AccountingOfficerRole
                };
            }
        }
    }

    private async Task GetTrustContactAddress(TrustDto trust, CancellationToken cancellationToken)
    {
        ContactAddress = string.Join(", ", new[]
        {
            trust.Address.Street,
            trust.Address.Locality,
            trust.Address.Town,
            trust.Address.County,
            trust.Address.Postcode
        }.Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    private async Task GetSchoolAccountingOfficer(string supportingOrganisationId, CancellationToken cancellationToken)
    {
        ContactAddress = SupportProject?.SupportingOrganisationAddress;

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
            Address = ContactAddress ?? "",
            RoleName = HeadteacherRole
        };
    }
}