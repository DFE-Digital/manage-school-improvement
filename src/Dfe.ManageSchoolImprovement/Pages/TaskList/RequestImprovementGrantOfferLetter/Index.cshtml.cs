using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.RequestImprovementGrantOfferLetter
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator, IConfiguration configuration) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
    {
        [BindProperty(Name = "grant-team-contacted-date", BinderType = typeof(DateInputModelBinder))]
        [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
        [Display(Name = "grant team contacted date")]
        public DateTime? GrantTeamContactedDate { get; set; }

        [BindProperty(Name = "include-contact-details")]
        public bool? IncludeContactDetails { get; set; }

        [BindProperty(Name = "attach-school-improvement-plan")]
        public bool? AttachSchoolImprovementPlan { get; set; }

        [BindProperty(Name = "copy-in-regional-director")]
        public bool? CopyInRegionalDirector { get; set; }

        [BindProperty(Name = "send-email-to-grant-team")]
        public bool? SendEmailToGrantTeam { get; set; }

        public string? EmailAddress { get; set; }

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

            var request = new SetRequestImprovementGrantOfferLetterCommand(new SupportProjectId(id), GrantTeamContactedDate, IncludeContactDetails, AttachSchoolImprovementPlan, CopyInRegionalDirector, SendEmailToGrantTeam);

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
            GrantTeamContactedDate = SupportProject.DateTeamContactedForRequestingImprovementGrantOfferLetter;
            IncludeContactDetails = SupportProject.IncludeContactDetails;
            AttachSchoolImprovementPlan = SupportProject.AttachSchoolImprovementPlan;
            CopyInRegionalDirector = SupportProject.CopyInRegionalDirector;
            SendEmailToGrantTeam = SupportProject.SendEmailToGrantTeam;

            EmailAddress = configuration.GetValue<string>("EmailForGrantOfferLetter") ?? string.Empty;
            return Page();
        }
    }
}
