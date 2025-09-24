using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;

public class AddEngagementConcern
{
    public record AddEngagementConcernCommand(
        SupportProjectId SupportProjectId,
        string? EngagementConcernDetails,
        DateTime? EngagementConcernRaisedDate,
        bool? EngagementConcernResolved,
        string? EngagementConcernResolvedDetails,
        DateTime? EngagementConcernResolvedDate
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
                request.EngagementConcernDetails,
                request.EngagementConcernRaisedDate,
                request.EngagementConcernResolved,
                request.EngagementConcernResolvedDetails,
                request.EngagementConcernResolvedDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}