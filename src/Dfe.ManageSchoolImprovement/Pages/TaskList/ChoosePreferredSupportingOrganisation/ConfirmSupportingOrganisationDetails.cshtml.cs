using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class ConfirmSupportingOrganisationDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public bool ShowError { get; set; }

    public string PreviousPage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, string? previousPage, CancellationToken cancellationToken = default)
    {
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        await base.GetSupportProject(id, cancellationToken);
        return Page();
    }

    public IActionResult OnPost(int id, string? previousPage)
    {
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        // This is a confirmation page - data is already saved from the entry page
        // Just redirect back to the task list
        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
    }
}
