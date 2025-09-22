using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.
    AddEngagementConcern;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.
    SetSupportProjectEngagementConcernEscalation;


namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class ChangeEngagementConcernModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "engagement-concern-details")]
    public string? EngagementConcernDetails { get; set; }

    [BindProperty(Name = "record-engagement-concern")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? RecordEngagementConcern { get; set; }

    public DateTime? DateEngagementConcernRaised { get; set; }
    
    public string? WarningNotice { get; set; }

    public bool ShowError => _errorService.HasErrors();

    private const string EngagementConcernDetailsKey = "engagement-concern-details";

    public bool ShowRecordEngagementConcernError => ModelState.ContainsKey(EngagementConcernDetailsKey) &&
                                                    ModelState[EngagementConcernDetailsKey]?.Errors.Count > 0;

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        RecordEngagementConcern = SupportProject.EngagementConcernRecorded;
        EngagementConcernDetails = SupportProject.EngagementConcernDetails;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        // set support project so we can compare values for success banner
        await base.GetSupportProject(id, cancellationToken);

        if (RecordEngagementConcern is true && string.IsNullOrEmpty(EngagementConcernDetails))
        {
            _errorService.AddError(EngagementConcernDetailsKey, "You must enter details");
            ModelState.AddModelError(EngagementConcernDetailsKey, "You must enter details");

            return Page();
        }

        TempData["EngagementConcernUpdated"] = SupportProject.EngagementConcernRecorded is true &&
                                               RecordEngagementConcern is true &&
                                               SupportProject.EngagementConcernDetails != EngagementConcernDetails;
        TempData["EngagementConcernRecorded"] =
            (SupportProject.EngagementConcernRecorded is null || SupportProject.EngagementConcernRecorded is false) &&
            RecordEngagementConcern is true;
        TempData["EngagementConcernRemoved"] =
            SupportProject.EngagementConcernRecorded is true && RecordEngagementConcern is false;

        DateEngagementConcernRaised = SupportProject.EngagementConcernRaisedDate ?? DateTime.Now;

        //reset details if removed
        if (RecordEngagementConcern is false)
        {
            RecordEngagementConcern = null;
            EngagementConcernDetails = null;
            DateEngagementConcernRaised = null;
            WarningNotice = null;
        }

        var request = new AddEngagementConcernCommand(new SupportProjectId(id),
            RecordEngagementConcern, EngagementConcernDetails, DateEngagementConcernRaised);

        var result = await mediator.Send(request, cancellationToken);

        if (result == false)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        if (SupportProject.EngagementConcernRecorded is true && RecordEngagementConcern is null)
        {
            var escalationRequest =
                new SetSupportProjectEngagementConcernEscalationCommand(new SupportProjectId(id), null, null, null,
                    null, null);
            var escalationResult = await mediator.Send(escalationRequest, cancellationToken);
            if (escalationResult == false)
            {
                _errorService.AddApiError();
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }
        }

        return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
    }
}