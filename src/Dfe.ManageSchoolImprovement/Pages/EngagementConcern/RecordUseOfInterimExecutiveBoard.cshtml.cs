using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectIebDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class RecordUseOfInterimExecutiveBoardModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public string ReturnPage { get; set; }

    [BindProperty(Name = "ieb-created-details")]
    public string? InterimExecutiveBoardCreatedDetails { get; set; }

    public bool ShowError => _errorService.HasErrors();

    public bool ShowDetailsError => ModelState.ContainsKey("ieb-created-details") &&
                                  ModelState["ieb-created-details"]?.Errors.Count > 0;

    [BindProperty(Name = "ieb-created")]
    [ModelBinder(BinderType = typeof(CheckboxInputModelBinder))]
    public bool? InterimExecutiveBoardCreated { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        ProjectListFilters.ClearFiltersFrom(TempData);

        ReturnPage = @Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        InterimExecutiveBoardCreated = SupportProject.InterimExecutiveBoardCreated;
        InterimExecutiveBoardCreatedDetails = SupportProject.InterimExecutiveBoardCreatedDetails;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (InterimExecutiveBoardCreated == true && string.IsNullOrEmpty(InterimExecutiveBoardCreatedDetails))
        {
            ModelState.AddModelError("ieb-created-details", "You must enter details");
        }

        if (InterimExecutiveBoardCreated != true)
        {
            InterimExecutiveBoardCreatedDetails = null;
        }

        _errorService.AddErrors(Request.Form.Keys, ModelState);
        if (_errorService.HasErrors()) return await base.GetSupportProject(id, cancellationToken);

        return RedirectToPage(@Links.EngagementConcern.RecordInterimExecutiveBoardDate.Page, new { id, InterimExecutiveBoardCreated, InterimExecutiveBoardCreatedDetails });
    }

}
