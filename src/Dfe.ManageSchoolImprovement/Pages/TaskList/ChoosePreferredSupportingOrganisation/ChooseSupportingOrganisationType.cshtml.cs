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
    public bool? CompleteAssessmentTool { get; set; }

    public bool ShowError { get; set; }
    public string AssessmentToolTwoLink { get; set; } = string.Empty;
    public string AssessmentToolTwoSharePointFolderLink { get; set; } = string.Empty;

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

        // Set default to "School" if no value is set
        SupportOrganisationType = SupportProject?.SupportOrganisationType ?? "School";

        await LoadSharePointLinksAsync(cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        // Load SharePoint links early for both success and error paths
        await LoadSharePointLinksAsync(cancellationToken);

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
            CompleteAssessmentTool);

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
            return Links.TaskList.EnterSupportingOrganisationSchoolDetails.Page;
        }
        else if (SupportOrganisationType == "Trust")
        {
            return Links.TaskList.EnterSupportingOrganisationTrustDetails.Page;
        }
        else if (SupportOrganisationType == "Local authority")
        {
            return Links.TaskList.EnterSupportingOrganisationLocalAuthorityDetails.Page;
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
