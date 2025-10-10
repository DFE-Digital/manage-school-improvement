using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.
    SetImprovementPlanObjectivesComplete;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan
{
    public class IndexModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService,
        IMediator mediator)
        : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty] public bool MarkAsComplete { get; set; }

        public ImprovementPlanViewModel? ImprovementPlan { get; set; }

        public List<ImprovementPlanObjectiveViewModel> QualityOfEducationObjectives =>
            ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Quality of education")
                .OrderBy(o => o.Order).ToList() ?? new();

        public List<ImprovementPlanObjectiveViewModel> LeadershipAndManagementObjectives =>
            ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Leadership and management")
                .OrderBy(o => o.Order).ToList() ?? new();

        public List<ImprovementPlanObjectiveViewModel> BehaviourAndAttitudesObjectives =>
            ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Behaviour and attitudes")
                .OrderBy(o => o.Order).ToList() ?? new();

        public List<ImprovementPlanObjectiveViewModel> AttendanceObjectives =>
            ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Attendance")
                .OrderBy(o => o.Order).ToList() ?? new();

        public List<ImprovementPlanObjectiveViewModel> PersonalDevelopmentObjectives =>
            ImprovementPlan?.ImprovementPlanObjectives?.Where(o => o.AreaOfImprovement == "Personal development")
                .OrderBy(o => o.Order).ToList() ?? new();

        public bool ShowMarkAsCompleteError => _errorService.GetError(nameof(MarkAsComplete)) != null;
        public bool ShowError => _errorService.HasErrors();

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            // Ensure SupportProject is not null before accessing ImprovementPlans
            if (SupportProject?.ImprovementPlans != null)
            {
                ImprovementPlan = SupportProject.ImprovementPlans.FirstOrDefault();
                if (ImprovementPlan != null)
                {
                    MarkAsComplete = ImprovementPlan.ObjectivesSectionComplete ?? false;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            var action = Request.Form["action"].ToString();

            if (action == "add-another")
            {
                // Redirect to select area page to add another objective
                return RedirectToPage(@Links.ImprovementPlan.SelectAnAreaOfImprovement.Page,
                    new { id, returnPage = @Links.ImprovementPlan.Index.Page });
            }

            await base.GetSupportProject(id, cancellationToken);
            // Ensure SupportProject is not null before accessing ImprovementPlans
            if (SupportProject?.ImprovementPlans != null)
            {
                ImprovementPlan = SupportProject.ImprovementPlans.FirstOrDefault();
            }

            // if we mark as complete, we need to ensure that at least one objective exists for Quality of Education or Leadership and Management
            if (MarkAsComplete && ImprovementPlan?.ImprovementPlanObjectives != null)
            {
                var hasQualityOfEducation =
                    ImprovementPlan.ImprovementPlanObjectives.Any(x => x.AreaOfImprovement == "Quality of education");
                var hasLeadershipAndManagement =
                    ImprovementPlan.ImprovementPlanObjectives.Any(x =>
                        x.AreaOfImprovement == "Leadership and management");

                if (!hasQualityOfEducation && !hasLeadershipAndManagement)
                {
                    _errorService.AddError(nameof(MarkAsComplete),
                        "Add at least one Quality of education objective, or at least one Leadership and management objective");
                }
            }


            if (!ModelState.IsValid || _errorService.HasErrors())
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                return await base.GetSupportProject(id, cancellationToken);
            }

            // Handle save and return
            var request = new SetImprovementPlanObjectivesCompleteCommand(new SupportProjectId(id),
                new ImprovementPlanId(ImprovementPlan!.Id), MarkAsComplete);
            var result = await mediator.Send(request, cancellationToken);

            TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
    }
}