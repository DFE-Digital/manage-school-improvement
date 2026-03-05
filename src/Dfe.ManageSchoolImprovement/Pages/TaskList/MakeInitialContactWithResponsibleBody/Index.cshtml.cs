using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.MakeInitialContactWithResponsibleBody;

public class MakeInitialContactWithResponsibleBodyModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "responsible-body-initial-contact-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Enter the date of initial contact")]
    public DateTime? ResponsibleBodyInitialContactDate { get; set; }

    [BindProperty(Name = "initial-contact-responsible-body")]
    [Display(Name = "Contact the responsible body")]
    public bool? InitialContactResponsibleBody { get; set; }

    public TaskListStatus? TaskListStatus { get; set; }
        
    public ProjectStatusValue? ProjectStatus { get; set; }
    
    public bool ShowError { get; set; }
    public string TargetedInterventionGuidanceLink { get; set; } = string.Empty;

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        await LoadGuidanceLinkAsync(cancellationToken);

        // Tuple deconstruction for property assignments
        (ResponsibleBodyInitialContactDate, InitialContactResponsibleBody) = (
            SupportProject?.InitialContactResponsibleBodyDate,
            SupportProject?.InitialContactResponsibleBody
        );
        
        TaskListStatus = TaskStatusViewModel.ContactedTheResponsibleBodyTaskStatus(SupportProject);
        ProjectStatus = SupportProject?.ProjectStatus;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        // Load guidance link early for both success and error paths
        await LoadGuidanceLinkAsync(cancellationToken);

        // Early return for validation errors
        if (!ResponsibleBodyInitialContactDate.HasValue)
        {
            ModelState.AddModelError("responsible-body-initial-contact-date", "Enter a date");
        }

        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        // Target-typed new expression (.NET 8)
        SetInitialContactTheResponsibleBodyDetailsCommand request = new(
            new SupportProjectId(id),
            InitialContactResponsibleBody,
            ResponsibleBodyInitialContactDate);

        var result = await mediator.Send(request, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
    }

    // Extracted method for loading guidance link
    private async Task LoadGuidanceLinkAsync(CancellationToken cancellationToken)
    {
        TargetedInterventionGuidanceLink = await sharePointResourceService
            .GetTargetedInterventionGuidanceLinkAsync(cancellationToken) ?? string.Empty;
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        await base.GetSupportProject(id, cancellationToken);
        return Page();
    }
}