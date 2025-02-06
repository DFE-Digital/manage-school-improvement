using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Queries;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.ValueObjects;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Models;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Pages.TaskList.ReviewTheImprovementPlan
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),IDateValidationMessageProvider
    {
        [BindProperty(Name = "date-improvement-plan-received",BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        public DateTime? DateImprovementPlanReceived { get; set; }

        [BindProperty(Name = "review-improvement-plan-with-team")]
        public bool? ReviewImprovementPlanWithTeam { get; set; } 
        
        public bool ShowError { get; set; }
        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }

        string IDateValidationMessageProvider.AllMissing(string displayName)
        {
            return $"Enter date the improvement plan was received.";
        }
        
        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            DateImprovementPlanReceived = SupportProject.ImprovementPlanReceivedDate;
            ReviewImprovementPlanWithTeam = SupportProject.ReviewImprovementPlanWithTeam;
            return Page();
        }
        
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }
            
            var request = new SetReviewTheImprovementPlanCommand(new SupportProjectId(id), DateImprovementPlanReceived, ReviewImprovementPlanWithTeam);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); ;
            }

            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
        
    }
}
