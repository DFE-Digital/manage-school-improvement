using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.
    AddEngagementConcern;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.
    SetSupportProjectEngagementConcernEscalation;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernResolvedDetails;


namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class AddEngagementConcernModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator,
    IDateTimeProvider dateTimeProvider) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "engagement-concern-details")]
    public string? EngagementConcernDetails { get; set; }

    public DateTime? DateEngagementConcernRaised { get; set; }

    public bool ShowError => _errorService.HasErrors();

    private const string EngagementConcernDetailsKey = "engagement-concern-details";

    public bool ShowRecordEngagementConcernError => ModelState.ContainsKey(EngagementConcernDetailsKey) &&
                                                    ModelState[EngagementConcernDetailsKey]?.Errors.Count > 0;

    [BindProperty(Name = "resolution-details")]
    public string? ResolutionDetails { get; set; }

    [BindProperty(Name = "mark-concern-resolved")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? MarkConcernResolved { get; set; }

    private const string ResolutionDetailsKey = "resolution-details";

    public bool ShowResolutionDetailsError => ModelState.ContainsKey(ResolutionDetailsKey) && ModelState[ResolutionDetailsKey]?.Errors.Count > 0;
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        // set support project so we can compare values for success banner
        await base.GetSupportProject(id, cancellationToken);

        if (string.IsNullOrEmpty(EngagementConcernDetails))
        {
            ModelState.AddModelError(EngagementConcernDetailsKey, "You must enter concern details");
        }

        if (MarkConcernResolved == true && string.IsNullOrWhiteSpace(ResolutionDetails))
        {
            ModelState.AddModelError(ResolutionDetailsKey, "You must enter resolution details");
        }

        if (!ModelState.IsValid)
        {
            this._errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }

        TempData["EngagementConcernRecorded"] = true;

        DateEngagementConcernRaised = dateTimeProvider.Now;

        var request = new AddEngagementConcernCommand(new SupportProjectId(id),
            EngagementConcernDetails, DateEngagementConcernRaised,
        MarkConcernResolved,
        MarkConcernResolved is null || MarkConcernResolved is false ? null : ResolutionDetails,
        MarkConcernResolved is null || MarkConcernResolved is false ? null : dateTimeProvider.Now);

        var result = await mediator.Send(request, cancellationToken);

        if (result == false)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

//         var resolveRequest = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
//                 // new EngagementConcernId(EngagementConcernId), 
//                 new SupportProjectId(id),
// );

        // var resolveResult = await mediator.Send(resolveRequest, cancellationToken);

        // if (!resolveResult)
        // {
        //     _errorService.AddApiError();
        //     await base.GetSupportProject(id, cancellationToken);
        //     return Page();
        // }

        return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
    }
}