using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Eligibility;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool
{
    public class EligibilityCheckDateModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService,
        IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
        IDateValidationMessageProvider
    {
        public string? ReturnPage { get; set; }
        
        [BindProperty(Name = "eligibility-check-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "When did the school's eligibility change?")]
        public DateTime? DateEligibilityChanged { get; set; }
        
        public bool ShowError { get; set; }

        string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
        {
            return $"Date must include a {string.Join(" and ", missingParts)}";
        }

        string IDateValidationMessageProvider.AllMissing => "Enter a date";


        public async Task<IActionResult> OnGet(int id, string? returnPage, CancellationToken cancellationToken)
        {
            ReturnPage = returnPage ?? @Links.AddSchool.ConfirmStartingEligibility.Page;
            await base.GetSupportProject(id, cancellationToken);
            
            DateEligibilityChanged = SupportProject?.DateEligibilityChanged;

            return Page();
        }

        public async Task<IActionResult> OnPost(int id, string? returnPage, CancellationToken cancellationToken)
        {
            ReturnPage = returnPage ?? @Links.AddSchool.EligibilityCheckDetails.Page;
            
            await base.GetSupportProject(id, cancellationToken);
            
            var request = new SetEligibilityCommand(new SupportProjectId(id), SupportProject?.SupportProjectEligibilityStatus, SupportProject?.SchoolIsNotEligibleNotes);

            var result = await mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            // TaskUpdated = true;
            return RedirectToPage(ReturnPage, new { id });
        }
    }
}