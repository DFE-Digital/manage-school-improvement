using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class EscalateEngagementConcernModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseEngagementConcernPageModel(supportProjectQueryService, errorService, mediator)
{
    // Fixed: Initialize with empty string to avoid CS8618
    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty(Name = "use-existing-relationships")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? UseExistingRelationships { get; set; }

    [BindProperty(Name = "follow-mediation-process")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? FollowMediationProcess { get; set; }

    [BindProperty(Name = "use-information-powers")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? UseInformationPowers { get; set; }

    [BindProperty(Name = "gather-evidence")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? GatherEvidence { get; set; }

    [BindProperty(Name = "get-approval")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? GetApproval { get; set; }

    [BindProperty(Name = "complete-moderation")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? CompleteModeration { get; set; }

    [BindProperty(Name = "draft-notice")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? DraftNotice { get; set; }

    [BindProperty(Name = "consult-la")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? ConsultLa { get; set; }

    [BindProperty(Name = "carry-out-assessment")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? CarryOutAssessment { get; set; }

    [BindProperty(Name = "submit-notice")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? SubmitNotice { get; set; }

    [BindProperty(Name = "notify-responsible-body")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? NotifyResponsibleBody { get; set; }

    [BindProperty(Name = "send-notice")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? SendNotice { get; set; }

    [BindProperty(Name = "complete-form")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? CompleteForm { get; set; }

    public bool? ConfirmStepsTaken { get; set; }

    [BindProperty]
    public Guid EngagementConcernId { get; set; }

    public bool ShowError => _errorService.HasErrors();
    public string SOPUCommissioningForm { get; set; } = string.Empty;

    // Static readonly properties for error constants
    public static string BeforeUsingNoticeError => "before-using-notice";
    public static string GetApprovalError => "get-approval";
    public static string IssuingNoticeError => "issuing-notice";

    // Expression-bodied properties for error display
    public bool ShowBeforeUsingNoticeError => ModelState.ContainsKey(BeforeUsingNoticeError) &&
                                             ModelState[BeforeUsingNoticeError].Errors.Count > 0;

    public bool ShowGetApprovalError => ModelState.ContainsKey(GetApprovalError) &&
                                       ModelState[GetApprovalError].Errors.Count > 0;

    public bool ShowIssuingNoticeError => ModelState.ContainsKey(IssuingNoticeError) &&
                                         ModelState[IssuingNoticeError]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, Guid engagementconcernid, CancellationToken cancellationToken = default)
    {
        ReturnPage = Links.EngagementConcern.Index.Page;
        EngagementConcernId = engagementconcernid;

        await base.GetSupportProject(id, cancellationToken);
        await LoadSOPUCommissioningFormAsync(cancellationToken);

        // Populate form fields from engagement concern data
        PopulateFormFields();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await LoadSOPUCommissioningFormAsync(cancellationToken);

        // Validate form data
        ValidateFormData();

        // Early return for validation errors
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        await base.GetSupportProject(id, cancellationToken);

        // Handle escalation post with target-typed new
        return await HandleEscalationPost(
            EngagementConcernId,
            id,
            new EngagementConcernEscalationDetails
            {
                ConfirmStepsTaken = true
            },
            cancellationToken: cancellationToken);
    }

    // Extracted method for loading SOPU commissioning form
    private async Task LoadSOPUCommissioningFormAsync(CancellationToken cancellationToken)
    {
        SOPUCommissioningForm = await sharePointResourceService
            .GetSOPUCommissioningFormLinkAsync(cancellationToken) ?? string.Empty;
    }

    // Extracted method for getting engagement concern
    private EngagementConcernViewModel? GetEngagementConcern()
    {
        return SupportProject?.EngagementConcerns?.FirstOrDefault(a => a.Id.Value == EngagementConcernId);
    }

    // Extracted method for populating form fields
    private void PopulateFormFields()
    {
        var engagementConcern = GetEngagementConcern();

        if (engagementConcern != null)
        {
            ConfirmStepsTaken = engagementConcern.EngagementConcernEscalationConfirmStepsTaken;
        }
    }

    // Extracted method for form validation logic
    private void ValidateFormData()
    {
        // Validation using collection expressions (.NET 8)
        var beforeNoticeChecks = new[] { UseExistingRelationships, FollowMediationProcess, UseInformationPowers };
        var approvalChecks = new[] { GatherEvidence, GetApproval, CompleteModeration, DraftNotice, ConsultLa, CarryOutAssessment, SubmitNotice };
        var issuingNoticeChecks = new[] { NotifyResponsibleBody, SendNotice, CompleteForm };

        if (beforeNoticeChecks.Any(check => check is false))
        {
            var errorMessage = "You must complete all actions in Before you consider using a notice";
            _errorService.AddError(BeforeUsingNoticeError, errorMessage);
            ModelState.AddModelError(BeforeUsingNoticeError, errorMessage);
        }

        if (approvalChecks.Any(check => check is false))
        {
            var errorMessage = "You must complete all actions in Get approval to use a notice";
            _errorService.AddError(GetApprovalError, errorMessage);
            ModelState.AddModelError(GetApprovalError, errorMessage);
        }

        if (issuingNoticeChecks.Any(check => check is false))
        {
            var errorMessage = "You must complete all actions in Issuing a notice";
            _errorService.AddError(IssuingNoticeError, errorMessage);
            ModelState.AddModelError(IssuingNoticeError, errorMessage);
        }
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        return Page();
    }

    protected internal override IActionResult GetDefaultRedirect(int id, object? routeValues = default)
    {
        return RedirectToPage(Links.EngagementConcern.ReasonForEscalation.Page,
            new { id, engagementConcernId = EngagementConcernId });
    }
}