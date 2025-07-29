using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.AddImprovementPlan;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans.AddImprovementPlanObjective;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan
{
    public class AddAnObjectiveModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty]
        public string? SelectedAreaOfImprovement { get; set; }

        [BindProperty(Name = nameof(ObjectiveDetails))]
        [Required(ErrorMessage = "Enter details of the objective")]
        [Display(Name = "Objective details")]
        public string? ObjectiveDetails { get; set; }

        public bool ShowDetailsError => ModelState.ContainsKey(nameof(ObjectiveDetails)) && ModelState[nameof(ObjectiveDetails)]?.Errors.Count > 0;

        public bool ShowError => _errorService.HasErrors();

        public string ReturnPage { get; private set; } = string.Empty;

        public async Task<IActionResult> OnGet(int id, string selectedAreaOfImprovement, string? returnPage, CancellationToken cancellationToken)
        {
            // If returnPage is not provided, check TempData for a previous return page
            var tempDataKey = $"ReturnPage_{nameof(Links.ImprovementPlan.AddAnObjective)}";
            returnPage ??= TempData[tempDataKey] as string;
            TempData[tempDataKey] = returnPage;

            ReturnPage = returnPage ?? Links.ImprovementPlan.SelectAnAreaOfImprovement.Page;

            await base.GetSupportProject(id, cancellationToken);
            SelectedAreaOfImprovement = selectedAreaOfImprovement;
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, string selectedAreaOfImprovement, CancellationToken cancellationToken)
        {
            SelectedAreaOfImprovement = selectedAreaOfImprovement;

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                return await base.GetSupportProject(id, cancellationToken);
            }
            // If we reach here, the model is valid, so we can proceed with adding the improvement plan and objective
            // Get the support project to load the existing improvement plans
            await base.GetSupportProject(id, cancellationToken);

            ImprovementPlanId improvementPlanId;

            if (SupportProject?.ImprovementPlans == null || !SupportProject.ImprovementPlans.Any())
            {
                // create a new improvement plan if it doesn't exist  
                var addPlanRequest = new AddImprovementPlanCommand(new SupportProjectId(id));
                improvementPlanId = await mediator.Send(addPlanRequest, cancellationToken);
            }
            else
            {
                improvementPlanId = new ImprovementPlanId(SupportProject.ImprovementPlans.First().Id);
            }

            //create objective
            var addObjectiveRequest = new AddImprovementPlanObjectiveCommand(
                new SupportProjectId(id),
                improvementPlanId,
                SelectedAreaOfImprovement,
                ObjectiveDetails!.Trim());

            var result = await mediator.Send(addObjectiveRequest, cancellationToken);

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
                // TODO: Create the review objectives page  
                return RedirectToPage(@Links.ImprovementPlan.Index.Page, new { id });
            }
        }


    }
}
