using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public class ProjectStatusPausedDateModel(ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "project-status-paused-date", BinderType = typeof(DateInputModelBinder))] 
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    public DateTime? PausedDate { get; set; }
    
    [BindProperty]
    public ProjectStatusValue ProjectStatus { get; set; }
    
    [BindProperty]
    public string? ChangedBy { get; set; }

    public bool ShowError { get; set; }
    
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
        
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, ProjectStatusValue projectStatus, string changedBy, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.ProjectStatusTab.Index.Page;

        await base.GetSupportProject(id, cancellationToken);
        
        ProjectStatus = projectStatus;
        ChangedBy = changedBy;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        if (!PausedDate.HasValue)
        {
            ModelState.AddModelError("project-status-paused-date", "Enter a date");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        return RedirectToPage(@Links.ProjectStatusTab.ProjectStatusPausedDetails.Page,
            new
            {
                id,
                projectStatus = ProjectStatus,
                changedBy = ChangedBy,
                pausedDate = PausedDate?.ToString("yyyy-MM-dd")
            });    
    }
    
}
