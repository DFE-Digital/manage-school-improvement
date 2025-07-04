using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages;

public class BaseSupportProjectPageModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : PageModel
{
    protected readonly ISupportProjectQueryService _supportProjectQueryService = supportProjectQueryService;
    protected readonly ErrorService _errorService = errorService;
    public SupportProjectViewModel? SupportProject { get; set; }
    
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
}
