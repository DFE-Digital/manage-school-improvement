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
            Guid engagementConcernId,
            int id,
            EngagementConcernEscalationDetails escalationDetails,
            object? routeValues = default,
            CancellationToken cancellationToken = default)
        {
            await base.GetSupportProject(id, cancellationToken);
            var engagementConcern = SupportProject?.EngagementConcerns?.FirstOrDefault(a => a.Id.Value == engagementConcernId);


            // If entering from change link, use existing values from SupportProject
            var details = escalationDetails with
            {
                ConfirmStepsTaken = escalationDetails.ConfirmStepsTaken ?? engagementConcern?.EngagementConcernEscalationConfirmStepsTaken,
                PrimaryReason = escalationDetails.PrimaryReason ?? engagementConcern?.EngagementConcernEscalationPrimaryReason,
                Details = escalationDetails.Details ?? engagementConcern?.EngagementConcernEscalationDetails,
                DateOfDecision = escalationDetails.DateOfDecision ?? engagementConcern?.EngagementConcernEscalationDateOfDecision,
                WarningNotice = escalationDetails.WarningNotice ?? engagementConcern?.EngagementConcernEscalationWarningNotice
            };

            var request = new SetSupportProjectEngagementConcernEscalationCommand(
                new EngagementConcernId(engagementConcernId),
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

            return GetDefaultRedirect(id, routeValues);
        }
    }
}

