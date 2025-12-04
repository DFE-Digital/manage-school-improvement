using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Extensions;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class ConfirmSupportingOrganisationDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    ITrustsClient trustClient,
    IDateTimeProvider dateTimeProvider,
    ErrorService errorService,
    IMediator mediator)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    [BindProperty(Name = "date-support-organisation-confirmed", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationConfirmed { get; set; }

    public string? OrganisationAddress { get; set; }

    [BindProperty]
    public ContactViewModel? AccountingOfficer { get; set; }
    
    public const string AccountingOfficerRole = "Accounting Officer";
    public bool ShowError { get; set; }

    public string? DateConfirmedErrorMessage { get; private set; }

    public string PreviousPage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, string? previousPage,
        CancellationToken cancellationToken = default)
    {
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        await base.GetSupportProject(id, cancellationToken);

        OrganisationAddress = SupportProject?.SupportingOrganisationAddress;
        DateSupportOrganisationConfirmed = SupportProject?.DateSupportOrganisationChosen;

        if (SupportProject?.SupportOrganisationType == "Trust" && !string.IsNullOrEmpty(SupportProject.SupportOrganisationName))
        {
            var trustContacts = await trustClient
                .GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(SupportProject.SupportOrganisationIdNumber,
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
                        RoleName = AccountingOfficerRole
                    };
                }
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, string? previousPage, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        if (DateSupportOrganisationConfirmed == null)
        {
            DateConfirmedErrorMessage = "Enter a date";
            ModelState.AddModelError("date-support-organisation-confirmed", DateConfirmedErrorMessage);
        }

        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        var dateContactDetailsAdded = DateTime.Now;

        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            SupportProject?.SupportOrganisationName,
            SupportProject?.SupportOrganisationIdNumber,
            SupportProject?.SupportOrganisationType, // OrganisationType is maintained from the previous page
            DateSupportOrganisationConfirmed,
            SupportProject?.AssessmentToolTwoCompleted,
            SupportProject?.SupportingOrganisationAddress,
            AccountingOfficer?.Name,
            AccountingOfficer?.Email,
            AccountingOfficer?.Phone,
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
    
    protected async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await GetSupportProject(id, cancellationToken);
    }
}