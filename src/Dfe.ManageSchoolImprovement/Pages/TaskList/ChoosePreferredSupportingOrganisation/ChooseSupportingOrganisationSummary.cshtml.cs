using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class ChooseSupportOrganisationSummaryModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public bool ShowError { get; set; }
    
    public string? SupportOrganisationType { get; set; }
    public string? SupportOrganisationName { get; set; }
    public string? SupportOrganisationIdNumber { get; set; }
    public DateTime? DateSupportingOrganisationContactDetailsAdded { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        SupportOrganisationType = SupportProject?.SupportOrganisationType;
        SupportOrganisationName = SupportProject?.SupportOrganisationName;
        SupportOrganisationIdNumber = SupportProject?.SupportOrganisationIdNumber;
        DateSupportingOrganisationContactDetailsAdded = SupportProject?.DateSupportingOrganisationContactDetailsAdded;
        
        return Page();
    }
}
