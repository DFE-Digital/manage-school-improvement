using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.SetImprovementPlanObjectiveDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan
{
    public class DeleteAnObjectiveModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        public string? SelectedAreaOfImprovement { get; set; }
        
        public string? ObjectiveDetails { get; set; }
        
        public int? ObjectiveOrder { get; set; }
        
        [BindProperty]
        public Guid ImprovementPlanId { get; set; }

        [BindProperty]
        public Guid ImprovementPlanObjectiveId { get; set; }

        public string ReturnPage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet(int id, int objectiveId, string? returnPage, CancellationToken cancellationToken)
        {
            ReturnPage = returnPage ?? Links.ImprovementPlan.Index.Page;

            await base.GetSupportProject(id, cancellationToken);

            var improvementPlan = SupportProject?.ImprovementPlans?.FirstOrDefault();

            if (improvementPlan != null)
            {
                var improvementPlanObjective = improvementPlan.ImprovementPlanObjectives?.FirstOrDefault(o => o.ReadableId == objectiveId);

                if (improvementPlanObjective != null)
                {
                    SelectedAreaOfImprovement = improvementPlanObjective.AreaOfImprovement;
                    ObjectiveDetails = improvementPlanObjective.Details;
                    ImprovementPlanId = improvementPlan.Id;
                    ImprovementPlanObjectiveId = improvementPlanObjective.Id;
                    ObjectiveOrder = improvementPlanObjective.Order;
                }
            }

            return Page();
        }
        public async Task<IActionResult> OnPost(int id, int objectiveId, string? returnPage, CancellationToken cancellationToken)
        {
            ReturnPage = returnPage ?? Links.ImprovementPlan.Index.Page;

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                return await base.GetSupportProject(id, cancellationToken);
            }

            var deleteObjectiveRequest = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommand(
                new SupportProjectId(id),
                new ImprovementPlanId(ImprovementPlanId),
                new ImprovementPlanObjectiveId(ImprovementPlanObjectiveId),
                User.Identity?.Name!);

            await mediator.Send(deleteObjectiveRequest, cancellationToken);

            TaskUpdated = true;

            // Redirect to review objectives page  
            return RedirectToPage(ReturnPage, new { id });

        }


    }
}
