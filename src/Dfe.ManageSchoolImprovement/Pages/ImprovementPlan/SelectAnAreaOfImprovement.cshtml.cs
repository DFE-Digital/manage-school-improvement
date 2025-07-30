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
        [BindProperty(Name = nameof(SelectedAreaOfImprovement))]
        [Required]
        [Display(Name = "Select an area of improvement")]
        public string? SelectedAreaOfImprovement { get; set; }
        public string? SelectedAreaOfImprovementErrorMessage { get; set; } = null;

        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public bool ShowSelectedAreaOfImprovementError => ModelState.ContainsKey(nameof(SelectedAreaOfImprovement)) && ModelState[nameof(SelectedAreaOfImprovement)]?.Errors.Count > 0;
        public bool ShowError => _errorService.HasErrors();
        public string ReturnPage { get; private set; } = string.Empty;
        public async Task<IActionResult> OnGet(int id, string? selectedAreaOfImprovement, string? returnPage, CancellationToken cancellationToken)
        {
            // If returnPage is not provided, check TempData for a previous return page
            var tempDataKey = $"ReturnPage_{nameof(Links.ImprovementPlan.SelectAnAreaOfImprovement)}";
            returnPage ??= TempData[tempDataKey] as string;
            TempData[tempDataKey] = returnPage;

            ReturnPage = returnPage ?? @Links.TaskList.Index.Page;

            await base.GetSupportProject(id, cancellationToken);
            SelectedAreaOfImprovement = selectedAreaOfImprovement;
            RadioButtons = RadioButtonModel;
            return Page();
        }
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                if (ShowSelectedAreaOfImprovementError)
                {
                    SelectedAreaOfImprovementErrorMessage = "Select an area of improvement";
                    _errorService.AddError(RadioButtonModel.First().Id, SelectedAreaOfImprovementErrorMessage);
                }

                RadioButtons = RadioButtonModel;
                _errorService.AddErrors(Request.Form.Keys, ModelState);

                return await base.GetSupportProject(id, cancellationToken);
            }

            return RedirectToPage(@Links.ImprovementPlan.AddAnObjective.Page, new { id, SelectedAreaOfImprovement });
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
