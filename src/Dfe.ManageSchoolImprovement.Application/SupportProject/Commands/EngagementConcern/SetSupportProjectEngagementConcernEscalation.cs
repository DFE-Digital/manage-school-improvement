using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote;


public class SetSupportProjectEngagementConcernEscalation
{
    public record SetSupportProjectEngagementConcernEscalationCommand(
        SupportProjectId SupportProjectId,
        bool? EngagementConcernEscalationConfirmStepsTaken,
        string? EngagementConcernEscalationPrimaryReason,
        string? EngagementConcernEscalationDetails,
        DateTime? EngagementConcernEscalationDateOfDecision
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

            supportProject.SetEngagementConcernEscalation(request.EngagementConcernEscalationConfirmStepsTaken, request.EngagementConcernEscalationPrimaryReason,
                request.EngagementConcernEscalationDetails, request.EngagementConcernEscalationDateOfDecision);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}