using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.SetImprovementPlanObjectiveDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan
{
    public class EditAnObjectiveModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty]
        public string? SelectedAreaOfImprovement { get; set; }

        [BindProperty(Name = nameof(ObjectiveDetails))]
        [Required(ErrorMessage = "Enter details of the objective")]
        [Display(Name = "Objective details")]
        public string? ObjectiveDetails { get; set; }

        public bool ShowDetailsError => ModelState.ContainsKey(nameof(ObjectiveDetails)) && ModelState[nameof(ObjectiveDetails)]?.Errors.Count > 0;

        public bool ShowError => _errorService.HasErrors();

        [BindProperty]
        public Guid ImprovementPlanId { get; set; }

        [BindProperty]
        public Guid ImprovementPlanObjectiveId { get; set; }

        public string? ObjectiveDetailsErrorMessage { get; set; } = null;

        public string ReturnPage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet(int id, int ObjectiveId, string returnPage, CancellationToken cancellationToken)
        {
            ReturnPage = returnPage ?? Links.ImprovementPlan.Index.Page;

            await base.GetSupportProject(id, cancellationToken);

            var improvementPlan = SupportProject?.ImprovementPlans?.FirstOrDefault();

            if (improvementPlan != null)
            {
                var improvementPlanObjective = improvementPlan.ImprovementPlanObjectives?.FirstOrDefault(o => o.ReadableId == ObjectiveId);

                if (improvementPlanObjective != null)
                {
                    SelectedAreaOfImprovement = improvementPlanObjective.AreaOfImprovement;
                    ObjectiveDetails = improvementPlanObjective.Details;
                    ImprovementPlanId = improvementPlan.Id;
                    ImprovementPlanObjectiveId = improvementPlanObjective.Id;
                }
            }

            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                return await base.GetSupportProject(id, cancellationToken);
            }

            //create objective
            var editObjectiveRequest = new SetImprovementPlanObjectiveDetailsCommand(
                new SupportProjectId(id),
                new ImprovementPlanId(ImprovementPlanId),
                new ImprovementPlanObjectiveId(ImprovementPlanObjectiveId),
                ObjectiveDetails!.Trim());

            var result = await mediator.Send(editObjectiveRequest, cancellationToken);

            TaskUpdated = true;

            // Get the action from the button clicked  
            var action = Request.Form["action"].ToString();

            // Determine where to redirect based on button clicked  
            if (action == "add-another")
            {
                // Redirect back to select area page to add another objective  
                return RedirectToPage(@Links.ImprovementPlan.SelectAnAreaOfImprovement.Page, new { id });
            }
            else // action == "review"  
            {
                // Redirect to review objectives page  
                return RedirectToPage(@Links.ImprovementPlan.Index.Page, new { id });
            }
        }


    }
}
