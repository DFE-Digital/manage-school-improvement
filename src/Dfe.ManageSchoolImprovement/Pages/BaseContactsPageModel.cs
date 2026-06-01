using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages;

public class BaseContactsPageModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : PageModel
{
    protected readonly ISupportProjectQueryService _supportProjectQueryService = supportProjectQueryService;
    protected readonly ErrorService _errorService = errorService;
    public SupportProjectViewModel? SupportProject { get; set; }

    public bool IsReadOnly => SupportProject?.ProjectStatus != ProjectStatusValue.InProgress;

    [TempData]
    public bool TaskUpdated { get; set; }

    public virtual async Task<IActionResult> GetSupportProjectWithContacts(int id, CancellationToken cancellationToken)
    {
        var result = await _supportProjectQueryService.GetSupportProjectWithContacts(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound();
        }

        SupportProject = SupportProjectViewModel.Create(result.Value!);
        return Page();
    }
}

