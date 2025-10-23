using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.SendFormalNotification;

public class SendFormalNotificationModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
    IDateValidationMessageProvider
{
    [BindProperty(Name = "use-enrolment-letter-template-to-draft-email")]
    public bool? UseEnrolmentLetterTemplateToDraftEmail { get; set; }

    [BindProperty(Name = "attach-targeted-intervention-information-sheet")]
    public bool? AttachTargetedInterventionInformationSheet { get; set; }

    [BindProperty(Name = "add-recipients")]
    public bool? AddRecipients { get; set; }

    [BindProperty(Name = "send-email")] public bool? SendEmail { get; set; }

    [BindProperty(Name = "date-of-formal-contact", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Enter the date of formal contact")]
    public DateTime? DateOfFormalContact { get; set; }

    public bool ShowError { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        UseEnrolmentLetterTemplateToDraftEmail = SupportProject.UseEnrolmentLetterTemplateToDraftEmail ?? null;
        AttachTargetedInterventionInformationSheet = SupportProject.AttachTargetedInterventionInformationSheet ?? null;
        AddRecipients = SupportProject.AddRecipientsForFormalNotification ?? null;
        SendEmail = SupportProject.FormalNotificationSent ?? null;
        DateOfFormalContact = SupportProject.DateFormalNotificationSent ?? null;

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

        var request = new SetSendFormalNotificationCommand(new SupportProjectId(id),
            UseEnrolmentLetterTemplateToDraftEmail, AttachTargetedInterventionInformationSheet, AddRecipients,
            SendEmail, DateOfFormalContact);

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