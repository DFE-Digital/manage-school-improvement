using System.ComponentModel.DataAnnotations;
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
    IApplicationSettingsResourceService applicationSettingsResourceService)
    : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "SupportOrganisationType")]
    public string? SupportOrganisationType { get; set; }
    
    [Display(Name = "Supporting organisation name")]
    public string? SupportOrganisationName { get; set; }
    
    [Display(Name = "Supporting organisation ID number")]
    public string? SupportOrganisationIdNumber { get; set; }
    
    [Display(Name = "Supporting organisation address")]
    public string? SupportOrganisationAddress { get; set; }
    
    [Display(Name = "Enter date supporting organisation confirmed")]
    public DateTime? DateSupportOrganisationChosen { get; set; }
    public TaskListStatus? TaskListStatus { get; set; }
    public ProjectStatusValue? ProjectStatus { get; set; }
    
    public string AssessmentToolTwoLink { get; set; } = string.Empty;

    [BindProperty(Name = "js-enabled")]
    public bool JavaScriptEnabled { get; set; }

    public bool ShowError { get; set; }
    public string? SupportOrganisationTypeErrorMessage { get; set; }
    public IList<RadioButtonsLabelViewModel> SupportOrganisationTypeOptions { get; set; } = CreateSupportOrganisationTypeOptions();

    // Expression-bodied interface implementations
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts) =>
        $"Date must include a {string.Join(" and ", missingParts)}";

    string IDateValidationMessageProvider.AllMissing =>
        "Enter a date";

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject != null)
        {
            SupportOrganisationType = SupportProject.SupportOrganisationType;
            
            TaskListStatus = TaskStatusViewModel.ChoosePreferredSupportingOrganisationTaskListStatus(SupportProject);
            ProjectStatus = SupportProject.ProjectStatus;
            SupportOrganisationName = SupportProject.SupportOrganisationName;
            SupportOrganisationIdNumber = SupportProject.SupportOrganisationIdNumber;
            SupportOrganisationAddress = SupportProject.SupportingOrganisationAddress;
            DateSupportOrganisationChosen = SupportProject.DatePreferredSupportOrganisationChosen;
        }
        
        AssessmentToolTwoLink =
            await applicationSettingsResourceService.GetAssessmentToolTwoLinkAsync(cancellationToken) ??
            string.Empty;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        await base.GetSupportProject(id, cancellationToken);
        
        var jsEnabled = Request.Form["js-enabled"].ToString();
        JavaScriptEnabled = jsEnabled.Equals("true", StringComparison.OrdinalIgnoreCase);

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
            SupportProject?.DatePreferredSupportOrganisationChosen,
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
        if (SupportOrganisationType == "Trust")
        {
            return JavaScriptEnabled ? Links.TaskList.EnterSupportingOrganisationTrustDetails.Page :
                Links.TaskList.EnterSupportingOrganisationTrustDetailsFallback.Page;
        }
        if (SupportOrganisationType == "Local authority")
        {
            return JavaScriptEnabled ? Links.TaskList.EnterSupportingOrganisationLocalAuthorityDetails.Page :
                Links.TaskList.EnterSupportingOrganisationLocalAuthorityDetailsFallback.Page;
        }
        if (SupportOrganisationType == "Local authority traded service") // Local authority traded service
        {
            return Links.TaskList.EnterSupportingOrganisationLocalAuthorityTradedServiceDetails.Page;
        }
        
        return Links.TaskList.EnterSupportingOrganisationFederationEducationPartnershipDetails.Page;
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
            },
            new()
            {
                Id = "support-organisation-type-federation-education-partnership",
                Name = "Federation or education partnership",
                Value = "Federation or education partnership"
            }
        };
    }
}
