using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetImprovementPlanTemplateDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ShareTheImprovementPlanTemplate;

public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "send-the-template-to-the-supporting-organisation")]
    public bool? SendTheTemplateToTheSupportingOrganisation { get; set; }

    [BindProperty(Name = "send-the-template-to-the-schools-responsible-body")]
    public bool? SendTheTemplateToTheSchoolsResponsibleBody { get; set; }

    [BindProperty(Name = "date-templates-sent", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date templates sent")]
    public DateTime? DateTemplatesSent { get; set; }

    public bool ShowError { get; set; }

    [BindProperty(Name = "calculate-funding-band")]
    public bool CalculateFundingBand { get; set; }

    [BindProperty(Name = "funding-band")]
    public string? FundingBand { get; set; }

    [BindProperty(Name = "send-template")]
    public bool SendTemplate { get; set; }

    public string? FundingBandErrorMessage { get; set; }

    public IList<RadioButtonsLabelViewModel> FundingBandOptions { get; set; } = new List<RadioButtonsLabelViewModel>
    {
        new()
        {
            Id = "funding-band-1",
            Name = "Reserves exceed funding level",
            Value = "reserves-exceed"
        },
        new()
        {
            Id = "funding-band-2",
            Name = "Up to £40,000",
            Value = "40000"
        },
        new()
        {
            Id = "funding-band-3",
            Name = "Up to £80,000",
            Value = "80000"
        },
        new()
        {
            Id = "funding-band-4",
            Name = "Up to £120,000",
            Value = "120000"
        }
    };

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the date templates sent";
    }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        SendTheTemplateToTheSupportingOrganisation = SupportProject.SendTheTemplateToTheSupportingOrganisation;

        SendTheTemplateToTheSchoolsResponsibleBody = SupportProject.SendTheTemplateToTheSchoolsResponsibleBody;

        DateTemplatesSent = SupportProject.DateTemplatesSent;

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

        var request = new SetImprovementPlanTemplateDetailsCommand(new SupportProjectId(id), SendTheTemplateToTheSupportingOrganisation, SendTheTemplateToTheSchoolsResponsibleBody, DateTemplatesSent);

        var result = await mediator.Send(request, cancellationToken);

        if (result != true)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken); ;
        }

        TaskUpdated = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }

}
