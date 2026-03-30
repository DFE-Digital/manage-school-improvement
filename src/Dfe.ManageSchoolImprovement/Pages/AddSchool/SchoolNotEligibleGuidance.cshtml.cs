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
    public class SchoolNotEligibleGuidanceModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService,
        IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
        IDateValidationMessageProvider
    {
        public SupportProjectEligibilityStatus? EligibilityStatus { get; set; }
        public DateTime? DateEligibilityChanged { get; set; }
        public string? EligibilityChangedDetails { get; set; }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            EligibilityStatus = SupportProject?.SupportProjectEligibilityStatus;
            DateEligibilityChanged = SupportProject?.DateEligibilityChanged;
            EligibilityChangedDetails = SupportProject?.SchoolIsNotEligibleNotes;

            return Page();
        }
        
        public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            
            var request = new SetEligibilityCommand(new SupportProjectId(id), 
                SupportProject?.SupportProjectEligibilityStatus, 
                SupportProject?.DateEligibilityChanged, 
                SupportProject?.SchoolIsNotEligibleNotes,
                true
            );

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }
            
            
            return RedirectToPage(@Links.SchoolList.Index.Page);
        }
    }
}