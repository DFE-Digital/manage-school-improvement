using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote;


public class SetSupportProjectEngagementConcernResolvedDetails
{
    public record SetSupportProjectEngagementConcernResolvedDetailsCommand(
        EngagementConcernId engagementConcernId,
        SupportProjectId SupportProjectId,
        bool? EngagementConcernResolved,
        string? EngagementConcernResolvedDetails,
        DateTime? EngagementConcernResolvedDate
    ) : IRequest<bool>;

    public class SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSupportProjectEngagementConcernResolvedDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetSupportProjectEngagementConcernResolvedDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetEngagementConcernResolvedDetails(request.engagementConcernId, request.EngagementConcernResolved, request.EngagementConcernResolvedDetails, request.EngagementConcernResolvedDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}