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
    
    public List<List<ImprovementPlanObjectiveViewModel>>Objectives { get; set; } = new();
    
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

            if (QualityOfEducationObjectives.Count != 0 ||
                LeadershipAndManagementObjectives.Count != 0 ||
                BehaviourAndAttitudesObjectives.Count != 0 ||
                AttendanceObjectives.Count != 0 ||
                PersonalDevelopmentObjectives.Count != 0)
            {
                Objectives = new List<List<ImprovementPlanObjectiveViewModel>>
                {
                    QualityOfEducationObjectives,
                    LeadershipAndManagementObjectives,
                    BehaviourAndAttitudesObjectives,
                    AttendanceObjectives,
                    PersonalDevelopmentObjectives,
                };  
            }
            else
            {
                Objectives = new List<List<ImprovementPlanObjectiveViewModel>>();
            }

        }

        return Page();
    }
}