using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;


public class SetSupportProjectEngagementConcernEscalation
{
    public record SetSupportProjectEngagementConcernEscalationCommand(
        EngagementConcernId engagementConcernId,
        SupportProjectId SupportProjectId,
        bool? EngagementConcernEscalationConfirmStepsTaken,
        string? EngagementConcernEscalationPrimaryReason,
        string? EngagementConcernEscalationDetails,
        DateTime? EngagementConcernEscalationDateOfDecision,
        string? EngagementConcernEscalationWarningNotice
    ) : IRequest<bool>;

    public class SetSupportProjectEngagementConcernEscalationCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSupportProjectEngagementConcernEscalationCommand, bool>
    {
        public async Task<bool> Handle(SetSupportProjectEngagementConcernEscalationCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetEngagementConcernEscalation(request.engagementConcernId, request.EngagementConcernEscalationConfirmStepsTaken, request.EngagementConcernEscalationPrimaryReason,
                request.EngagementConcernEscalationDetails, request.EngagementConcernEscalationDateOfDecision, request.EngagementConcernEscalationWarningNotice);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}