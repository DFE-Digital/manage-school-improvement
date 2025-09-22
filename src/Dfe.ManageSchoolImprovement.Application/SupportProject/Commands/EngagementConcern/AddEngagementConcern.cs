using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;

public class AddEngagementConcern
{
    public record AddEngagementConcernCommand(
        SupportProjectId SupportProjectId,
        bool? EngagementConcernRecorded,
        string? EngagementConcernDetails,
        DateTime? EngagementConcernRaisedDate
    ) : IRequest<bool>;

    public class AddEngagementConcernCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<AddEngagementConcernCommand, bool>
    {
        public async Task<bool> Handle(AddEngagementConcernCommand request, CancellationToken cancellationToken)
        {
            var supportProject =
                await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            var engagementConcernId = new EngagementConcernId(Guid.NewGuid());

            supportProject.AddEngagementConcern(engagementConcernId, request.SupportProjectId,
                request.EngagementConcernRecorded, request.EngagementConcernDetails,
                request.EngagementConcernRaisedDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}