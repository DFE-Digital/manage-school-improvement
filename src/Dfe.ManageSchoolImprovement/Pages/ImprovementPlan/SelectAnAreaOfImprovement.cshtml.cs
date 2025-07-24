using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ImprovementPlan
{
    public class SelectAnAreaOfImprovementModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "SelectedAreaOfImprovement")]
        [Required]
        [Display(Name = "Select an area of improvement")]
        public string? SelectedAreaOfImprovement { get; set; }

        public string? SelectedAreaOfImprovementErrorMessage { get; set; } = null;

        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public bool ShowError { get; set; }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            SelectedAreaOfImprovement = null; // Will be set when user selects an option
            RadioButtons = RadioButtonModel;
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(SelectedAreaOfImprovement))
            {
                if (string.IsNullOrEmpty(SelectedAreaOfImprovement))
                {
                    SelectedAreaOfImprovementErrorMessage = "Select an area of improvement";
                    _errorService.AddError("SelectedAreaOfImprovement", "You must select an area of improvement");
                }

                RadioButtons = RadioButtonModel;
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;

                return await base.GetSupportProject(id, cancellationToken);
            }

            return RedirectToPage(@Links.ImprovementPlan.AddAnObject.Page, new { id, SelectedAreaOfImprovement });
        }


        private IList<RadioButtonsLabelViewModel> RadioButtonModel
        {
            get
            {
                var list = new List<RadioButtonsLabelViewModel>
                {
                    new() {
                        Id = "quality-of-education",
                        Name = "Quality of education",
                        Value = "Quality of education"
                    },
                    new() {
                        Id = "leadership-and-management",
                        Name = "Leadership and management",
                        Value = "Leadership and management"
                    },
                    new() {
                        Id = "behaviour-and-attitudes",
                        Name = "Behaviour and attitudes",
                        Value = "Behaviour and attitudes"
                    },
                    new() {
                        Id = "attendance",
                        Name = "Attendance",
                        Value = "Attendance"
                    },
                    new() {
                        Id = "personal-development",
                        Name = "Personal development",
                        Value = "Personal development"
                    }
                };

                return list;
            }
        }
    }
}
