using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool
{
    public class EligibilityCheckDetailsModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService,
        IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
        IDateValidationMessageProvider
    {
        [BindProperty(Name = "eligibility-check-details")]
        [Display(Name = "Explain the reasons for the eligibility change")]
        public string? SchoolIsNotEligibleNotes { get; set; }
        
        private const string SchoolIsNotEligibleNotesKey = "eligibility-check-details";
        
        public bool ShowError => _errorService.HasErrors();
        


        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            
            SchoolIsNotEligibleNotes = SupportProject?.SchoolIsNotEligibleNotes;

            return Page();
        }

        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            
            if (string.IsNullOrEmpty(SchoolIsNotEligibleNotes))
            {
                ModelState.AddModelError(SchoolIsNotEligibleNotesKey, "Enter details");
            }
            
            if (SchoolIsNotEligibleNotes?.Length > 500)
            {
                ModelState.AddModelError(SchoolIsNotEligibleNotesKey, "Details must be 500 characters or less");
            }
            
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                return await base.GetSupportProject(id, cancellationToken);
            }
            
            var request = new SetEligibilityCommand(new SupportProjectId(id), SupportProjectEligibilityStatus.NotEligibleForSupport, SupportProject?.DateEligibilityChanged, SchoolIsNotEligibleNotes);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            // TaskUpdated = true;
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
    }
}