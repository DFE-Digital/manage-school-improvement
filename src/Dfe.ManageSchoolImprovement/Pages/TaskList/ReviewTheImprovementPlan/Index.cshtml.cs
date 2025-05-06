using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ReviewTheImprovementPlan
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),IDateValidationMessageProvider
    {
        [BindProperty(Name = "date-improvement-plan-received",BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        public DateTime? DateImprovementPlanReceived { get; set; }

        [BindProperty(Name = "review-improvement-plan-with-team")]
        public bool? ReviewImprovementPlanWithTeam { get; set; } 
        
        [BindProperty(Name = "send-improvement-plan-to-rise")]
        public bool? SendImprovementPlanToRiseGrantTeam { get; set; } 

        [BindProperty(Name = "confirm-plan-cleared-by-rise")]
        public bool? ConfirmPlanClearedByRiseGrantTeam { get; set; } 
        
        public string EmailAddress { get; set; } = "rise.grant@education.gov.uk";
        
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
            SendImprovementPlanToRiseGrantTeam = SupportProject.SendImprovementPlanToRiseGrantTeam;
            ConfirmPlanClearedByRiseGrantTeam = SupportProject.ConfirmPlanClearedByRiseGrantTeam;
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

            var request = new SetReviewTheImprovementPlanCommand(new SupportProjectId(id), DateImprovementPlanReceived,
                ReviewImprovementPlanWithTeam, SendImprovementPlanToRiseGrantTeam, ConfirmPlanClearedByRiseGrantTeam);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); ;
            }

            TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
        
    }
}
