using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan;

public class ImprovementPlanTabModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    ErrorService errorService)
    : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
{
    public string ReturnPage { get; set; }

    public ImprovementPlanViewModel? ImprovementPlan { get; set; }
    public bool IsProjectAssigned { get; set; }
    public bool IsAdviserAllocated { get; set; }

    public bool NoObjectivesRecorded => ImprovementPlan?.ImprovementPlanObjectives == null || ImprovementPlan.ImprovementPlanObjectives.Count == 0;

    public List<ImprovementPlanObjectiveViewModel> QualityOfEducationObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Quality of education").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> LeadershipAndManagementObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Leadership and management").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> BehaviourAndAttitudesObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Behaviour and attitudes").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> AttendanceObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Attendance").OrderBy(o => o.Order).ToList() ?? new();

    public List<ImprovementPlanObjectiveViewModel> PersonalDevelopmentObjectives =>
        ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Personal development").OrderBy(o => o.Order).ToList() ?? new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ReturnPage = Links.SchoolList.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        // Ensure SupportProject is not null before accessing ImprovementPlans
        if (SupportProject?.ImprovementPlans != null)
        {
            ImprovementPlan = SupportProject.ImprovementPlans.FirstOrDefault();
            IsAdviserAllocated = SupportProject.AdviserEmailAddress != null;
            IsProjectAssigned = SupportProject.AssignedDeliveryOfficerEmailAddress != null;
        }

        return Page();
    }
}