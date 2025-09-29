using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectIebDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class RecordInterimExecutiveBoardDateModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    public string ReturnPage { get; set; }

    public bool ShowError => _errorService.HasErrors();

    [BindProperty(Name = "ieb-created-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Required]
    public DateTime? InterimExecutiveBoardCreatedDate { get; set; }

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"You must enter a date";
    }

    public async Task<IActionResult> OnGetAsync(int id, int readableEngagementConcernId, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == readableEngagementConcernId);

        if (engagementConcern != null)
        {
            InterimExecutiveBoardCreatedDate = engagementConcern.InterimExecutiveBoardCreatedDate;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id,
        int readableEngagementConcernId,
        bool? interimExecutiveBoardCreated,
        string? interimExecutiveBoardCreatedDetails,
        CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (!InterimExecutiveBoardCreatedDate.HasValue)
        {
            ModelState.AddModelError("ieb-created-date", "You must enter a date");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            if (_errorService.HasErrors()) return await base.GetSupportProject(id, cancellationToken);
        }

        var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == readableEngagementConcernId);

        if (engagementConcern?.Id == null)
        {
            throw new InvalidOperationException($"Engagement concern with readable ID {readableEngagementConcernId} not found");
        }

        var request = new SetSupportProjectIebDetailsCommand(
            engagementConcern.Id,
            new SupportProjectId(id),
            interimExecutiveBoardCreated,
            interimExecutiveBoardCreatedDetails,
            InterimExecutiveBoardCreatedDate);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }

        TempData["InterimExecutiveBoardRecorded"] = (engagementConcern.InterimExecutiveBoardCreated == null || engagementConcern.InterimExecutiveBoardCreated == false) && interimExecutiveBoardCreated == true;
        TempData["InterimExecutiveBoardDateUpdated"] = engagementConcern.InterimExecutiveBoardCreatedDate is not null && engagementConcern.InterimExecutiveBoardCreatedDate != InterimExecutiveBoardCreatedDate;

        return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
    }

}
