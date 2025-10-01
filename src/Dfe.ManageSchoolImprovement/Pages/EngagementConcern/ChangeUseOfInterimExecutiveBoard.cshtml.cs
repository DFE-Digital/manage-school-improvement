using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectIebDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class ChangeUseOfInterimExecutiveBoardModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "ieb-created-details")]
    public string? InterimExecutiveBoardCreatedDetails { get; set; }

    private DateTime? InterimExecutiveBoardCreatedDate { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public bool ShowDetailsError => ModelState.ContainsKey("ieb-created-details") &&
                                    ModelState["ieb-created-details"]?.Errors.Count > 0;

    [BindProperty(Name = "ieb-created")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? InterimExecutiveBoardCreated { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == readableEngagementConcernId);
        if (engagementConcern != null)
        {
            InterimExecutiveBoardCreated = engagementConcern.InterimExecutiveBoardCreated;
            InterimExecutiveBoardCreatedDetails = engagementConcern.InterimExecutiveBoardCreatedDetails;
        }


        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == readableEngagementConcernId);

        if (engagementConcern?.Id == null)
        {
            throw new InvalidOperationException($"Engagement concern with readable ID {readableEngagementConcernId} not found");
        }

        InterimExecutiveBoardCreatedDate = engagementConcern.InterimExecutiveBoardCreatedDate;

        if (InterimExecutiveBoardCreated == true && string.IsNullOrEmpty(InterimExecutiveBoardCreatedDetails))
        {
            ModelState.AddModelError("ieb-created-details", "You must enter details");
        }

        if (InterimExecutiveBoardCreated != true)
        {
            InterimExecutiveBoardCreatedDetails = null;
            InterimExecutiveBoardCreatedDate = null;
        }

        _errorService.AddErrors(Request.Form.Keys, ModelState);
        if (_errorService.HasErrors()) return await base.GetSupportProject(id, cancellationToken);

        var request = new SetSupportProjectIebDetailsCommand(
            engagementConcern.Id,
            new SupportProjectId(id),
            InterimExecutiveBoardCreated,
            InterimExecutiveBoardCreatedDetails,
            InterimExecutiveBoardCreatedDate);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TempData["InterimExecutiveBoardUpdated"] = engagementConcern.InterimExecutiveBoardCreated is true && InterimExecutiveBoardCreated is true &&
                                               engagementConcern.InterimExecutiveBoardCreatedDetails != InterimExecutiveBoardCreatedDetails;
        TempData["InterimExecutiveBoardRecorded"] = (engagementConcern.InterimExecutiveBoardCreated == null || engagementConcern.InterimExecutiveBoardCreated == false) && InterimExecutiveBoardCreated == true;
        TempData["InterimExecutiveBoardRemoved"] = engagementConcern.InterimExecutiveBoardCreated == true && InterimExecutiveBoardCreated == false;

        if (InterimExecutiveBoardCreated != true)
        {
            return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
        }

        return RedirectToPage(@Links.EngagementConcern.RecordInterimExecutiveBoardDate.Page, new { id, InterimExecutiveBoardCreated, InterimExecutiveBoardCreatedDetails });
    }

}
