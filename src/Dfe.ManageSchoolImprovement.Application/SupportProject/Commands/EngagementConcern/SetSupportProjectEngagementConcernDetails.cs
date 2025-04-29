using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote;


public class SetSupportProjectEngagementConcernDetails
{
    public record SetSupportProjectEngagementConcernDetailsCommand(
        SupportProjectId SupportProjectId,
        bool? EngagementConcernRecorded,
        string? EngagementConcernDetails
    ) : IRequest<bool>;

    public class SetSupportProjectEngagementConcernDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSupportProjectEngagementConcernDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetSupportProjectEngagementConcernDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetEngagementConcernDetails(request.EngagementConcernRecorded, request.EngagementConcernDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}