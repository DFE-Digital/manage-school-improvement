using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages;

public class BaseImprovementPlanPageModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : PageModel
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

        var result = await _supportProjectQueryService.GetSupportProjectImprovementPlanAllData(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        SupportProject = CreateSupportProject(result.Value!);
        return Page();
    }
    
    public virtual async Task<IActionResult> GetSupportProjectImprovementPlanAndObjectives(int id, CancellationToken cancellationToken)
    {
        return await GetProjectWithImprovementPlanAndObjectives(id, cancellationToken);
    }
    protected async Task<IActionResult> GetProjectWithImprovementPlanAndObjectives(int id, CancellationToken cancellationToken)
    {

        var result = await _supportProjectQueryService.GetSupportProjectImprovementPlanAndObjectives(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        SupportProject = CreateSupportProject(result.Value!);;
        return Page();
    }
    
    public virtual async Task<IActionResult> GetSupportProjectProgressReviews(int id, CancellationToken cancellationToken)
    {
        return await GetProjectWithProgressReviews(id, cancellationToken);
    }
    protected async Task<IActionResult> GetProjectWithProgressReviews(int id, CancellationToken cancellationToken)
    {

        var result = await _supportProjectQueryService.GetImprovementPlanProgressReviews(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        SupportProject = CreateSupportProject(result.Value!);
        return Page();
    }
    
    public virtual async Task<IActionResult> GetSupportProjectSummary(int id, CancellationToken cancellationToken)
    {
        var result = await _supportProjectQueryService.GetSupportProjectSummary(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }
        
        SupportProjectSummary = SupportProjectSummaryViewModel.Create(result.Value!);
        return Page();
    }
    
    private static SupportProjectViewModel CreateSupportProject(SupportProjectDto summary)
    {
        return SupportProjectViewModel.Create(summary);
    }
}
