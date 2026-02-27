using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RecordTheResponsibleBodyResponse
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = nameof(ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate), BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "Date the response was received")]
        public DateTime? ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate { get; set; }

        [BindProperty(Name = nameof(ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint))]
        [Display(Name = "Response saved in SharePoint")]
        public bool? ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint { get; set; }

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
            ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint = SupportProject.ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint;
            ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate = SupportProject.ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate;
            
            TaskListStatus = TaskStatusViewModel.ResponsibleBodyResponseToTheConflictOfInterestRequestStatus(SupportProject);
            ProjectStatus = SupportProject?.ProjectStatus;
            
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

            var request = new SetResponsibleBodyResponseToTheConflictOfInterestRequestCommand(
                new SupportProjectId(id),
                ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate,
                ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint);

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
