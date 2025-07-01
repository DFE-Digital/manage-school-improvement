using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernEscalation;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern
{
    public abstract class BaseEngagementConcernPageModel : BaseSupportProjectPageModel
    {
        protected readonly IMediator _mediator;

        protected BaseEngagementConcernPageModel(
            ISupportProjectQueryService supportProjectQueryService,
            ErrorService errorService,
            IMediator mediator) : base(supportProjectQueryService, errorService)
        {
            _mediator = mediator;
        }

        // Abstract method to be implemented by each page
        protected abstract IActionResult GetDefaultRedirect(int id, object routeValues = null);

        protected async Task<IActionResult> HandleEscalationPost(
            int id,
            bool? confirmStepsTaken,
            string? primaryReason,
            string? escalationDetails,
            DateTime? dateOfDecision,
            bool? changeLinkClicked,
            object routeValues = null,
            CancellationToken cancellationToken = default)
        {
            await base.GetSupportProject(id, cancellationToken);

            // If entering from change link, use existing values from SupportProject
            confirmStepsTaken ??= SupportProject.EngagementConcernEscalationConfirmStepsTaken;
            primaryReason ??= SupportProject.EngagementConcernEscalationPrimaryReason;
            escalationDetails ??= SupportProject.EngagementConcernEscalationDetails;
            dateOfDecision ??= SupportProject.EngagementConcernEscalationDateOfDecision;

            var request = new SetSupportProjectEngagementConcernEscalationCommand(
                new SupportProjectId(id),
                confirmStepsTaken,
                primaryReason,
                escalationDetails,
                dateOfDecision);

            var result = await _mediator.Send(request, cancellationToken);

            if (result == null)
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

