using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RecordTheSchoolResponse
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "school-response-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "school response")]
        public DateTime? SchoolResponseDate { get; set; }

        [BindProperty(Name = "HasAcceptedTargetedSupport")]
        public bool? HasAcceptedTargetedSupport { get; set; }

        [BindProperty(Name = "has-saved-school-response-in-sharepoint")]
        public bool? HasSavedSchoolResponseinSharePoint { get; set; }

        public required IList<RadioButtonsLabelViewModel> TargetedSupportRadioButtoons { get; set; }

        public bool ShowError { get; set; }

        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }

        string IDateValidationMessageProvider.AllMissing(string displayName)
        {
            return $"Enter the saved school's resonpse date in SharePoint";
        }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            HasAcceptedTargetedSupport = SupportProject.HasAcceptedTargetedSupport;
            HasSavedSchoolResponseinSharePoint = SupportProject.HasSavedSchoolResponseinSharePoint;
            SchoolResponseDate = SupportProject.SchoolResponseDate;
            TargetedSupportRadioButtoons = GetRadioButtons();
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                TargetedSupportRadioButtoons = GetRadioButtons();
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new SetSchoolResponseCommand(new SupportProjectId(id), SchoolResponseDate, HasAcceptedTargetedSupport, HasSavedSchoolResponseinSharePoint);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken); ;
            }

            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }

        private static IList<RadioButtonsLabelViewModel> GetRadioButtons()
        {
            var list = new List<RadioButtonsLabelViewModel>
            {
                new() {
                    Id = "accepted-intervention",
                    Name = "Accepted intervention",
                    Value = "True"
                },
                new() {
                    Id = "declined-intervention",
                    Name = "Declined intervention",
                    Value = "False"
                }
            };

            return list;
        }
    }
}
