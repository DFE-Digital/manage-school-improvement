using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.SendFormalNotification;

public class SendFormalNotificationModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "use-enrolment-letter-template-to-draft-email")]
    public bool? UseEnrolmentLetterTemplateToDraftEmail { get; set; }

    [BindProperty(Name = "attach-targeted-intervention-information-sheet")]
    public bool? AttachTargetedInterventionInformationSheet { get; set; }

    [BindProperty(Name = "add-recipients")]
    public bool? AddRecipients { get; set; }

    [BindProperty(Name = "send-email")]
    public bool? SendEmail { get; set; }

    [BindProperty(Name = "date-of-formal-contact", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Enter the date of formal contact")]
    public DateTime? DateOfFormalContact { get; set; }

    public bool ShowError { get; set; }
    public string EnrolmentLetterTemplate { get; set; } = string.Empty;

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing(string displayName) =>
        "Enter the school contacted date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        await LoadEnrolmentLetterTemplateAsync(cancellationToken);

        // Populate form fields from support project data
        PopulateFormFields();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        // Load template early for both success and error paths
        await LoadEnrolmentLetterTemplateAsync(cancellationToken);

        // Early return for validation errors
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        // Target-typed new expression (.NET 8)
        SetSendFormalNotificationCommand request = new(
            new SupportProjectId(id),
            UseEnrolmentLetterTemplateToDraftEmail,
            AttachTargetedInterventionInformationSheet,
            AddRecipients,
            SendEmail,
            DateOfFormalContact);

        var result = await mediator.Send(request, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TaskUpdated = true;
        return RedirectToPage(Links.TaskList.Index.Page, new { id });
    }

    // Extracted method for loading enrolment letter template
    private async Task LoadEnrolmentLetterTemplateAsync(CancellationToken cancellationToken)
    {
        EnrolmentLetterTemplate = await sharePointResourceService
            .GetEnrolmentLetterTemplateLinkAsync(cancellationToken) ?? string.Empty;
    }

    // Extracted method for populating form fields
    private void PopulateFormFields()
    {
        if (SupportProject is null) return;

        // Tuple deconstruction for property assignments
        (UseEnrolmentLetterTemplateToDraftEmail, AttachTargetedInterventionInformationSheet, AddRecipients, SendEmail, DateOfFormalContact) = (
            SupportProject.UseEnrolmentLetterTemplateToDraftEmail,
            SupportProject.AttachTargetedInterventionInformationSheet,
            SupportProject.AddRecipientsForFormalNotification,
            SupportProject.FormalNotificationSent,
            SupportProject.DateFormalNotificationSent
        );
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        await base.GetSupportProject(id, cancellationToken);
        return Page();
    }
}