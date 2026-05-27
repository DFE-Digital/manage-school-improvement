using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages;

public class BaseSupportProjectPageModel(ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService) : PageModel
{
    protected readonly ISupportProjectQueryService _supportProjectQueryService = supportProjectQueryService;
    protected readonly ErrorService _errorService = errorService;
    public SupportProjectViewModel? SupportProject { get; set; }
    
    public SupportProjectSummaryViewModel? SupportProjectSummary { get; set; }

    public bool IsReadOnly => SupportProject?.ProjectStatus != ProjectStatusValue.InProgress;

    [TempData]
    public bool TaskUpdated { get; set; }

    public virtual async Task<IActionResult> GetSupportProject(int id, CancellationToken cancellationToken)
    {
        return await GetProject(id, cancellationToken);
    }
    protected async Task<IActionResult> GetProject(int id, CancellationToken cancellationToken)
    {

        var result = await _supportProjectQueryService.GetSupportProject(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        SupportProject = SupportProjectViewModel.Create(result.Value!);
        return Page();
    }
    
    public virtual async Task<IActionResult> GetBaseSupportProject(int id, CancellationToken cancellationToken)
    {
        return await GetBaseProject(id, cancellationToken);
    }
    protected async Task<IActionResult> GetBaseProject(int id, CancellationToken cancellationToken)
    {

        var result = await _supportProjectQueryService.GetBaseSupportProject(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        SupportProject = SupportProjectViewModel.Create(result.Value!);
        return Page();
    }
    
    public virtual async Task<IActionResult> GetSupportProjectSummary(int id, CancellationToken cancellationToken)
    {
        return await GetProjectSummary(id, cancellationToken);
    }
    protected async Task<IActionResult> GetProjectSummary(int id, CancellationToken cancellationToken)
    {

        var result = await _supportProjectQueryService.GetSupportProjectSummary(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        SupportProjectSummary = SupportProjectSummaryViewModel.Create(result.Value!);
        return Page();
    }
    
    
    
    public virtual async Task UpdateCurrentDeliveryMilestone(int id, Milestone? currentDeliveryMilestone, Milestone newDeliveryMilestone,
        CancellationToken cancellationToken)
    {
        var newMilestoneValue = (int)newDeliveryMilestone;
        var currentMilestoneValue = (int?)currentDeliveryMilestone;
        if (!currentMilestoneValue.HasValue || newMilestoneValue > currentMilestoneValue.Value)
        {
            var request = new SetCurrentDeliveryMilestoneCommand(new SupportProjectId(id), newDeliveryMilestone);
        
            var mediator = HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator;
            var result = await mediator!.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
            }
        }
    }
    
}
