using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;

public class SetSupportProjectInformationPowersDetails
{
    public record SetSupportProjectInformationPowersDetailsCommand(
        EngagementConcernId EngagementConcernId,
        SupportProjectId SupportProjectId,
        bool? InformationPowersInUse,
        string? InformationPowersDetails,
        DateTime? PowersUsedDate
    ) : IRequest<bool>;

    public class SetSupportProjectInformationPowersDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSupportProjectInformationPowersDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetSupportProjectInformationPowersDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetInformationPowersDetails(
                request.EngagementConcernId,
                request.InformationPowersInUse,
                request.InformationPowersDetails,
                request.PowersUsedDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
