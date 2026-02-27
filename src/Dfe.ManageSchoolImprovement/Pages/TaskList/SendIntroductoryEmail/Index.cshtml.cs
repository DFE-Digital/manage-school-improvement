using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.SendIntroductoryEmail
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "introductory-email-sent-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "Date introductory email sent")]
        public DateTime? IntroductoryEmailSentDate { get; set; }

        [BindProperty(Name = "remind-adviser-to-copy-in-rise-team-on-email-sent")]
        [Display(Name = "Remind adviser to copy in RISE team")]
        public bool? RemindAdviserToCopyRiseTeamWhenSentEmail { get; set; }

        public TaskListStatus? TaskListStatus { get; set; }
        public ProjectStatusValue? ProjectStatus { get; set; }
        
        public bool ShowError { get; set; }

        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }
        
        string IDateValidationMessageProvider.AllMissing => "Enter a date";
        
        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (SupportProject != null)
            {
                RemindAdviserToCopyRiseTeamWhenSentEmail = SupportProject.RemindAdviserToCopyRiseTeamWhenSentEmail;
                IntroductoryEmailSentDate = SupportProject.IntroductoryEmailSentDate;
            
                TaskListStatus = TaskStatusViewModel.SendIntroductoryEmailTaskListStatus(SupportProject);
                ProjectStatus = SupportProject.ProjectStatus; 
            }
            
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

            var request = new SetSendIntroductoryEmailCommand(new SupportProjectId(id), IntroductoryEmailSentDate, RemindAdviserToCopyRiseTeamWhenSentEmail);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
    }
}
