using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ChoosePreferredSupportingOrganisation;

public class ChooseSupportOrganisationTypeModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    ISharePointResourceService sharePointResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "SupportOrganisationType")]
    public string? SupportOrganisationType { get; set; }

    [BindProperty(Name = "complete-assessment-tool")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? CompleteAssessmentTool { get; set; }

    [BindProperty(Name = "js-enabled")]
    public bool JavaScriptEnabled { get; set; }

    public bool ShowError { get; set; }
    public static string CompleteAssessmentToolError => "complete-assessment-tool";
    public bool ShowCompleteAssessmentToolError => ModelState.ContainsKey(CompleteAssessmentToolError) &&
                                              ModelState[CompleteAssessmentToolError].Errors.Count > 0;
    public string AssessmentToolTwoLink { get; set; } = string.Empty;
    public string AssessmentToolTwoSharePointFolderLink { get; set; } = string.Empty;
    public string? SupportOrganisationTypeErrorMessage { get; set; }
    public string? AssessmentToolTwoErrorMessage { get; set; }

    public IList<RadioButtonsLabelViewModel> SupportOrganisationTypeOptions { get; set; } = CreateSupportOrganisationTypeOptions();

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        CompleteAssessmentTool = SupportProject?.AssessmentToolTwoCompleted;
        SupportOrganisationType = SupportProject?.SupportOrganisationType;

        await LoadSharePointLinksAsync(cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        var jsEnabled = Request.Form["js-enabled"].ToString();
        JavaScriptEnabled = jsEnabled.Equals("true", StringComparison.OrdinalIgnoreCase);

        // Load SharePoint links early for both success and error paths
        await LoadSharePointLinksAsync(cancellationToken);
        
        if (CompleteAssessmentTool == null || CompleteAssessmentTool == false)
        {
            AssessmentToolTwoErrorMessage = "Confirm you have completed the assessment tool";
            ModelState.AddModelError(CompleteAssessmentToolError, AssessmentToolTwoErrorMessage);
            _errorService.AddError(CompleteAssessmentToolError, AssessmentToolTwoErrorMessage);
        }

        if (SupportOrganisationType == null)
        {
            SupportOrganisationTypeErrorMessage = "Select the type of supporting organisation";
            ModelState.AddModelError("support-organisation-type-school", SupportOrganisationTypeErrorMessage);
            _errorService.AddError("support-organisation-type-school", SupportOrganisationTypeErrorMessage);
        }
        
        // Early return for validation errors
        if (!ModelState.IsValid)
            return await HandleValidationErrorAsync(id, cancellationToken);

        var previousSupportOrganisationType = SupportProject?.SupportOrganisationType;

        var command = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            SupportProject?.SupportOrganisationName,
            SupportProject?.SupportOrganisationIdNumber,
            SupportOrganisationType,
            SupportProject?.DateSupportOrganisationChosen,
            CompleteAssessmentTool,
            SupportProject?.SupportingOrganisationAddress,
            SupportProject?.SupportingOrganisationContactName,
            SupportProject?.SupportingOrganisationContactEmailAddress,
            SupportProject?.SupportingOrganisationContactPhone,
            SupportProject?.SupportingOrganisationAddress);

        var result = await mediator.Send(command, cancellationToken);

        // Early return for API error
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;

        var nextPage = GetNextPage();

        return RedirectToPage(nextPage, new { id, previousSupportOrganisationType });
    }

    private string GetNextPage()
    {
        if (SupportOrganisationType == "School")
        {
            return JavaScriptEnabled ? Links.TaskList.EnterSupportingOrganisationSchoolDetails.Page :
                Links.TaskList.EnterSupportingOrganisationSchoolDetailsFallback.Page;
        }
        else if (SupportOrganisationType == "Trust")
        {
            return JavaScriptEnabled ? Links.TaskList.EnterSupportingOrganisationTrustDetails.Page :
                Links.TaskList.EnterSupportingOrganisationTrustDetailsFallback.Page;
        }
        else if (SupportOrganisationType == "Local authority")
        {
            return JavaScriptEnabled ? Links.TaskList.EnterSupportingOrganisationLocalAuthorityDetails.Page :
                Links.TaskList.EnterSupportingOrganisationLocalAuthorityDetailsFallback.Page;
        }
        else // Local authority traded service
        {
            return Links.TaskList.EnterSupportingOrganisationLocalAuthorityTradedServiceDetails.Page;
        }
    }

    // Extracted method for loading SharePoint links concurrently
    private async Task LoadSharePointLinksAsync(CancellationToken cancellationToken)
    {
        AssessmentToolTwoLink = await sharePointResourceService.GetAssessmentToolTwoLinkAsync(cancellationToken) ?? string.Empty;
        AssessmentToolTwoSharePointFolderLink = await sharePointResourceService.GetAssessmentToolTwoSharePointFolderLinkAsync(cancellationToken) ?? string.Empty;
    }

    // Extracted method for cleaner error handling
    private async Task<IActionResult> HandleValidationErrorAsync(int id, CancellationToken cancellationToken)
    {
        _errorService.AddErrors(Request.Form.Keys, ModelState);
        ShowError = true;
        return await base.GetSupportProject(id, cancellationToken);
    }

    private static IList<RadioButtonsLabelViewModel> CreateSupportOrganisationTypeOptions()
    {
        return new List<RadioButtonsLabelViewModel>
        {
            new()
            {
                Id = "support-organisation-type-school",
                Name = "School",
                Value = "School"
            },
            new()
            {
                Id = "support-organisation-type-trust",
                Name = "Trust",
                Value = "Trust"
            },
            new()
            {
                Id = "support-organisation-type-local-authority",
                Name = "Local authority",
                Value = "Local authority"
            },
            new()
            {
                Id = "support-organisation-type-local-authority-traded-service",
                Name = "Local authority traded service",
                Value = "Local authority traded service"
            }
        };
    }
}
