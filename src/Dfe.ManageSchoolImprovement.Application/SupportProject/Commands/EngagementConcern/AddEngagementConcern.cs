using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;

public class AddEngagementConcern
{
    public record AddEngagementConcernCommand(
        SupportProjectId SupportProjectId,
        string? EngagementConcernDetails,
        string? EngagementConcernSummary,
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
            var details = new EngagementConcernDetails
            {
                Details = request.EngagementConcernDetails,
                Summary = request.EngagementConcernSummary,
                RaisedDate = request.EngagementConcernRaisedDate,
                Resolved = request.EngagementConcernResolved,
                ResolvedDetails = request.EngagementConcernResolvedDetails,
                ResolvedDate = request.EngagementConcernResolvedDate,
            };

            supportProject.AddEngagementConcern(engagementConcernId, request.SupportProjectId,
                details);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}