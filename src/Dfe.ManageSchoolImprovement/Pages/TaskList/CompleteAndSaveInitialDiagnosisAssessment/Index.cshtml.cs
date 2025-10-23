using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc; 
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.CompleteAndSaveInitialDiagnosisAssessment
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "saved-assessemnt-template-in-sharepoint-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "Saved assessement template")]
        public DateTime? SavedAssessmentTemplateInSharePointDate { get; set; }

        [BindProperty(Name = "has-talk-to-adviser")]
        public bool? HasTalkToAdviserAboutFindings { get; set; }

        [BindProperty(Name = "complete-assessment-template")]
        public bool? HasCompleteAssessmentTemplate { get; set; }

        public bool ShowError { get; set; }

        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }
        
        string IDateValidationMessageProvider.AllMissing => "Enter a date";

        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetCompleteAndSaveInitialDiagnosisTemplateCommand(new SupportProjectId(id), SavedAssessmentTemplateInSharePointDate, HasTalkToAdviserAboutFindings, HasCompleteAssessmentTemplate);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); 
            }

            TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            SavedAssessmentTemplateInSharePointDate = SupportProject.SavedAssessmentTemplateInSharePointDate;
            HasTalkToAdviserAboutFindings = SupportProject.HasTalkToAdviserAboutFindings;
            HasCompleteAssessmentTemplate = SupportProject.HasCompleteAssessmentTemplate;
            return Page();
        }
    }
}
