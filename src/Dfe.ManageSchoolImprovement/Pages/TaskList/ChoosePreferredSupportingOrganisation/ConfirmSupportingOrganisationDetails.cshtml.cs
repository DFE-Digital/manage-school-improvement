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
    [BindProperty(Name = "date-support-organisation-confirmed", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? DateSupportOrganisationConfirmed { get; set; }
    
    public string? OrganisationAddress { get; set; }
    public bool ShowError { get; set; }
    
    public string? DateConfirmedErrorMessage { get; private set; }

    public string PreviousPage { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, string? previousPage, CancellationToken cancellationToken = default)
    {
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;

        await base.GetSupportProject(id, cancellationToken);

        OrganisationAddress = SupportProject?.SupportingOrganisationAddress;
        return Page();
    }

    public IActionResult OnPost(int id, string? previousPage)
    {
        PreviousPage = previousPage ?? Links.TaskList.ChoosePreferredSupportingOrganisationType.Page;
        
        if (DateSupportOrganisationConfirmed == null)
        {
            DateConfirmedErrorMessage = "Enter a date";
            ModelState.AddModelError("date-support-organisation-confirmed", DateConfirmedErrorMessage);
        }
        
        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
    }
}
