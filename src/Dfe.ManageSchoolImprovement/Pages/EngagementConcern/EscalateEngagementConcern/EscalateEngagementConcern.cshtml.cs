using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class EscalateEngagementConcernModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseEngagementConcernPageModel(supportProjectQueryService, errorService, mediator)
{
    public string ReturnPage { get; set; }
    
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
    
    public static string BeforeUsingNoticeError => "before-using-notice";
    
    public bool ShowBeforeUsingNoticeError => ModelState.ContainsKey(BeforeUsingNoticeError) && ModelState[BeforeUsingNoticeError].Errors.Count > 0;
    
    public static string GetApprovalError => "get-approval";
    
    public bool ShowGetApprovalError => ModelState.ContainsKey(GetApprovalError) && ModelState[GetApprovalError].Errors.Count > 0;
    
    public static string IssuingNoticeError => "issuing-notice";
    
    public bool ShowIssuingNoticeError => ModelState.ContainsKey(IssuingNoticeError) && ModelState[IssuingNoticeError]?.Errors.Count > 0;


    public async Task<IActionResult> OnGetAsync(int id, Guid engagementconcernid, CancellationToken cancellationToken)
    {
        ReturnPage = Links.EngagementConcern.Index.Page;
        
        await base.GetSupportProject(id, cancellationToken);
        
        EngagementConcernId = engagementconcernid;
        
        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(a => a.Id.Value == EngagementConcernId);

        if (engagementConcern != null)
        {
            ConfirmStepsTaken = engagementConcern.EngagementConcernEscalationConfirmStepsTaken;
        }
        
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (UseExistingRelationships is false || FollowMediationProcess is false || UseInformationPowers is false)
        {
            _errorService.AddError(BeforeUsingNoticeError, "You must complete all actions in Before you consider using a notice");
            ModelState.AddModelError(BeforeUsingNoticeError, "You must complete all actions in Before you consider using a notice");
        }
        
        if (GatherEvidence is false || 
            GetApproval is false || 
            CompleteModeration is false ||
            DraftNotice is false ||
            ConsultLa is false ||
            CarryOutAssessment is false ||
            SubmitNotice is false)
        {
            _errorService.AddError(GetApprovalError, "You must complete all actions in Get approval to use a notice");
            ModelState.AddModelError(GetApprovalError, "You must complete all actions in Get approval to use a notice");
        }
        
        if (NotifyResponsibleBody is false || SendNotice is false || CompleteForm is false)
        {
            _errorService.AddError(IssuingNoticeError, "You must complete all actions in Issuing a notice");
            ModelState.AddModelError(IssuingNoticeError, "You must complete all actions in Issuing a notice");
        }
        
        if (!ModelState.IsValid)
        {
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
        
        await base.GetSupportProject(id, cancellationToken);
        
        return await HandleEscalationPost(
            EngagementConcernId,
            id,
            new EngagementConcernEscalationDetails
                {
                    ConfirmStepsTaken = true
                },
            cancellationToken: cancellationToken);
        
        return RedirectToPage(@Links.EngagementConcern.ReasonForEscalation.Page, new { id, EngagementConcernId, ConfirmStepsTaken });
    }
    
    protected internal override IActionResult GetDefaultRedirect(int id, object? routeValues = default)
    {
        return RedirectToPage(@Links.EngagementConcern.ReasonForEscalation.Page, new { id, engagementConcernId=EngagementConcernId });
    }
}