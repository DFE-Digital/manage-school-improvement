using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool
{
    public class SchoolNotEligibleGuidanceModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
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
    }
}