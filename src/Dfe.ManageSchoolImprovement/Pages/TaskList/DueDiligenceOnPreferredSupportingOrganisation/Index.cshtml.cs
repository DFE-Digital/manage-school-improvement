using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetDueDiligenceOnPreferredSupportingOrganisationDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.DueDiligenceOnPreferredSupportingOrganisation;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator,
    ISharePointResourceService sharePointResourceService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "check-organisation-has-capacity-and-willing-to-provide-support")]
    public bool? CheckOrganisationHasCapacityAndWillingToProvideSupport { get; set; }

    [BindProperty(Name = "speak-to-trust-relationship-manager-or-local-authority-lead-to-check-choice")]
    public bool? CheckChoiceWithTrustRelationshipManagerOrLaLead { get; set; }

    [BindProperty(Name = "contact-sfso-for-financial-check")]
    public bool? ContactSfsoForFinancialCheck { get; set; }

    [BindProperty(Name = "check-the-organisation-has-a-vendor-account")]
    public bool? CheckTheOrganisationHasAVendorAccount { get; set; }

    [BindProperty(Name = "due-diligence-completed-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date due diligence completed")]
    public DateTime? DateDueDiligenceCompleted { get; set; }

    public bool ShowError { get; set; }

    public string CheckSupportingOrgVendorAccountLink { get; set; } = string.Empty;
    public string SFSOCommissioningFormLink { get; set; } = string.Empty;

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        CheckOrganisationHasCapacityAndWillingToProvideSupport = SupportProject.CheckOrganisationHasCapacityAndWillingToProvideSupport;
        CheckChoiceWithTrustRelationshipManagerOrLaLead = SupportProject.CheckChoiceWithTrustRelationshipManagerOrLaLead;
        ContactSfsoForFinancialCheck = SupportProject.DiscussChoiceWithSfso;
        CheckTheOrganisationHasAVendorAccount = SupportProject.CheckTheOrganisationHasAVendorAccount;
        DateDueDiligenceCompleted = SupportProject.DateDueDiligenceCompleted;

        CheckSupportingOrgVendorAccountLink = await sharePointResourceService.GetCheckSupportingOrganisationVendorAccountLink(cancellationToken) ?? string.Empty;
        SFSOCommissioningFormLink = await sharePointResourceService.GetSFSOCommissioningFormLink(cancellationToken) ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
    {
        CheckSupportingOrgVendorAccountLink = await sharePointResourceService.GetCheckSupportingOrganisationVendorAccountLink(cancellationToken) ?? string.Empty;
        SFSOCommissioningFormLink = await sharePointResourceService.GetSFSOCommissioningFormLink(cancellationToken) ?? string.Empty;

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }

        var request = new SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommand(
            new SupportProjectId(id),
            CheckOrganisationHasCapacityAndWillingToProvideSupport,
            CheckChoiceWithTrustRelationshipManagerOrLaLead,
            ContactSfsoForFinancialCheck,
            CheckTheOrganisationHasAVendorAccount,
            DateDueDiligenceCompleted);

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
