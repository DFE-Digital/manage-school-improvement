using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;

public class EditEngagementConcern
{
    public record EditEngagementConcernCommand(
        EngagementConcernId EngagementConcernId,
        SupportProjectId SupportProjectId,
        bool? EngagementConcernRecorded,
        string? EngagementConcernDetails,
        DateTime? EngagementConcernRaisedDate
    ) : IRequest<bool>;

    public class EditdEngagementConcernCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<EditEngagementConcernCommand, bool>
    {
        public async Task<bool> Handle(EditEngagementConcernCommand request, CancellationToken cancellationToken)
        {
            var supportProject =
                await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }
            

            supportProject.EditEngagementConcern(request.EngagementConcernId,
                request.EngagementConcernRecorded, request.EngagementConcernDetails,
                request.EngagementConcernRaisedDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}