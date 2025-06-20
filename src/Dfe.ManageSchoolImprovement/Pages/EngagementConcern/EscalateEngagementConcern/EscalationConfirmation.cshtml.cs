using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;

public class EscalationConfirmationModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService, 
    IConfiguration configuration) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }
    
    public string CoastingNotificationLetterLink { get; set; } = string.Empty;
    
    public string NoticeToEnterIntoArrangementsLink { get; set; } = string.Empty;
    
    public string TerminationWarningNoticeLink { get; set; } = string.Empty;
    
    public bool ShowError => _errorService.HasErrors();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.EngagementConcern.Index.Page;
        
        await base.GetSupportProject(id, cancellationToken);
        
        CoastingNotificationLetterLink = configuration.GetValue<string>("CoastingNotificationLetterLink") ?? string.Empty;
        NoticeToEnterIntoArrangementsLink = configuration.GetValue<string>("NoticeToEnterIntoArrangementsLink") ?? string.Empty;
        TerminationWarningNoticeLink = configuration.GetValue<string>("TerminationWarningNoticeLink") ?? string.Empty;
        
        return Page();
    }
}