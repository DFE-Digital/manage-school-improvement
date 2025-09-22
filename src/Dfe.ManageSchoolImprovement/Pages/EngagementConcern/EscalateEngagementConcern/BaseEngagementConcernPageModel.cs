using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectEngagementConcernEscalation;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern
{
    public abstract class BaseEngagementConcernPageModel : BaseSupportProjectPageModel
    {
        protected readonly IMediator _mediator;

        private protected BaseEngagementConcernPageModel(
           ISupportProjectQueryService supportProjectQueryService,
           ErrorService errorService,
           IMediator mediator) : base(supportProjectQueryService, errorService)
        {
            _mediator = mediator;
        }

        // Abstract method to be implemented by each page
        protected internal abstract IActionResult GetDefaultRedirect(int id, object? routeValues = default);

        protected internal async Task<IActionResult> HandleEscalationPost(
            int id,
            EngagementConcernEscalationDetails escalationDetails,
            bool? changeLinkClicked,
            object? routeValues = default,
            CancellationToken cancellationToken = default)
        {
            await base.GetSupportProject(id, cancellationToken);

            // If entering from change link, use existing values from SupportProject
            var details = escalationDetails with
            {
                ConfirmStepsTaken = escalationDetails.ConfirmStepsTaken ?? SupportProject?.EngagementConcernEscalationConfirmStepsTaken,
                PrimaryReason = escalationDetails.PrimaryReason ?? SupportProject?.EngagementConcernEscalationPrimaryReason,
                Details = escalationDetails.Details ?? SupportProject?.EngagementConcernEscalationDetails,
                DateOfDecision = escalationDetails.DateOfDecision ?? SupportProject?.EngagementConcernEscalationDateOfDecision,
                WarningNotice = escalationDetails.WarningNotice ?? SupportProject?.EngagementConcernEscalationWarningNotice
            };

            var request = new SetSupportProjectEngagementConcernEscalationCommand(
                new SupportProjectId(id),
                details.ConfirmStepsTaken,
                details.PrimaryReason,
                details.Details,
                details.DateOfDecision,
                details.WarningNotice);

            var result = await _mediator.Send(request, cancellationToken);

            if (!result)
            {
                _errorService.AddApiError();
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }

            if (changeLinkClicked == true)
            {
                return RedirectToPage(@Links.EngagementConcern.Index.Page, new { id });
            }

            return GetDefaultRedirect(id, routeValues);
        }
    }
}

