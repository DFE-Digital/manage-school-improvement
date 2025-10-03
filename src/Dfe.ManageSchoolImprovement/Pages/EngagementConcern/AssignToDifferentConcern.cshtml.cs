using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectIebDetails;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectInformationPowersDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern;

public class AssignToDifferentConcernModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
{
    public const string AssignIebToDifferentConcern = "Ieb";
    public const string AssignInfoPowersToDifferentConcern = "InfoPowers";

    public string ReturnPage { get; set; } = string.Empty;

    [BindProperty(Name = "SelectedConcernId")]
    public int? SelectedConcernId { get; set; }

    public List<EngagementConcernViewModel>? AvailableConcerns { get; set; } = null;

    public bool ShowError => _errorService.HasErrors();

    private const string ConcernSelectionKey = "SelectedConcernId-0";

    public bool ShowConcernSelectionError => ModelState.ContainsKey(ConcernSelectionKey) &&
                                             ModelState[ConcernSelectionKey]?.Errors.Count > 0;

    public string AssignType { get; private set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id, string? returnPage, int readableEngagementConcernId, string assignType, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        SetAvailableConcerns(assignType, readableEngagementConcernId);

        return Page();
    }

    private void SetAvailableConcerns(string assignType, int readableEngagementConcernId)
    {
        AssignType = assignType;

        if (assignType == AssignIebToDifferentConcern)
        {
            AvailableConcerns = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InterimExecutiveBoardCreated != true).OrderBy(x => x.EngagementConcernRaisedDate).ToList();
        }
        else if (assignType == AssignInfoPowersToDifferentConcern)
        {
            AvailableConcerns = SupportProject?.EngagementConcerns?.Where(x => x.EngagementConcernResolved != true && x.InformationPowersInUse != true).OrderBy(x => x.EngagementConcernRaisedDate).ToList();
        }

        // add the current concern to the list so it is clear which one it is currently assigned to
        if (AvailableConcerns != null && AvailableConcerns.Any())
        {
            if (SupportProject?.EngagementConcerns != null)
            {
                var concernToAdd = SupportProject.EngagementConcerns.SingleOrDefault(x => x.ReadableId == readableEngagementConcernId);
                if (concernToAdd != null)
                {
                    SelectedConcernId = concernToAdd.ReadableId;
                    AvailableConcerns.Add(concernToAdd);
                }
            }
        }

    }

    public async Task<IActionResult> OnPostAsync(int id, string? returnPage, int readableEngagementConcernId, string assignType, CancellationToken cancellationToken)
    {
        ReturnPage = returnPage ?? Links.EngagementConcern.Index.Page;

        await base.GetSupportProject(id, cancellationToken);

        if (!SelectedConcernId.HasValue)
        {
            ModelState.AddModelError(ConcernSelectionKey, $"Select which concern the {(assignType == AssignInfoPowersToDifferentConcern ? "Information powers" : "Interim executive board")} is related to");
        }

        if (!ModelState.IsValid)
        {
            // Create a list that includes both form keys AND our manual error key
            var allKeys = Request.Form.Keys.Union([ConcernSelectionKey]).ToList();

            SetAvailableConcerns(assignType, readableEngagementConcernId);
            this._errorService.AddErrors(allKeys, ModelState);
            return Page();
        }

        var currentEngagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == readableEngagementConcernId);
        var newEngagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(ec => ec.ReadableId == SelectedConcernId);

        if (currentEngagementConcern?.Id == null)
        {
            throw new InvalidOperationException($"Engagement concern with readable ID {readableEngagementConcernId} not found");
        }

        if (newEngagementConcern?.Id == null)
        {
            throw new InvalidOperationException($"Engagement concern with readable ID {SelectedConcernId} not found");
        }

        var supportProjectId = new SupportProjectId(id);

        if (assignType == AssignIebToDifferentConcern)
        {
            // Transfer IEB details from current to new concern
            var transferCommand = new SetSupportProjectIebDetailsCommand(
                newEngagementConcern.Id,
                supportProjectId,
                currentEngagementConcern.InterimExecutiveBoardCreated,
                currentEngagementConcern.InterimExecutiveBoardCreatedDetails,
                currentEngagementConcern.InterimExecutiveBoardCreatedDate);

            var clearCommand = new SetSupportProjectIebDetailsCommand(
                currentEngagementConcern.Id,
                supportProjectId,
                null, null, null);

            if (!await ExecuteCommandsAsync(transferCommand, clearCommand, cancellationToken))
            {
                return await HandleCommandError(id, cancellationToken);
            }
        }
        else if (assignType == AssignInfoPowersToDifferentConcern)
        {
            // Transfer Information Powers details from current to new concern
            var transferCommand = new SetSupportProjectInformationPowersDetailsCommand(
                newEngagementConcern.Id,
                supportProjectId,
                currentEngagementConcern.InformationPowersInUse,
                currentEngagementConcern.InformationPowersDetails,
                currentEngagementConcern.PowersUsedDate);

            var clearCommand = new SetSupportProjectInformationPowersDetailsCommand(
                currentEngagementConcern.Id,
                supportProjectId,
                null, null, null);

            if (!await ExecuteCommandsAsync(transferCommand, clearCommand, cancellationToken))
            {
                return await HandleCommandError(id, cancellationToken);
            }
        }

        return RedirectToPage(Links.EngagementConcern.Index.Page, new { id });
    }

    // Helper method to execute commands and handle common error logic
    private async Task<bool> ExecuteCommandsAsync<T>(T transferCommand, T clearCommand, CancellationToken cancellationToken)
        where T : IRequest<bool>
    {
        // Execute transfer command
        var transferResult = await mediator.Send(transferCommand, cancellationToken);
        if (!transferResult) return false;

        // Execute clear command
        var clearResult = await mediator.Send(clearCommand, cancellationToken);
        return clearResult;
    }

    // Helper method to handle command execution errors
    private async Task<IActionResult> HandleCommandError(int id, CancellationToken cancellationToken)
    {
        _errorService.AddApiError();
        await base.GetSupportProject(id, cancellationToken);
        return Page();
    }
}
