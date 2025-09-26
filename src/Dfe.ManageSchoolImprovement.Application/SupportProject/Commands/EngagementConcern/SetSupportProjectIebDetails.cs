using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;

public class SetSupportProjectIebDetails
{
    public record SetSupportProjectIebDetailsCommand(
        EngagementConcernId EngagementConcernId,
        SupportProjectId SupportProjectId,
        bool? InterimExecutiveBoardCreated,
        string? InterimExecutiveBoardCreatedDetails,
        DateTime? InterimExecutiveBoardCreatedDate
    ) : IRequest<bool>;

    public class SetSupportProjectIebDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSupportProjectIebDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetSupportProjectIebDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetInterimExecutiveBoardCreated(
                request.EngagementConcernId,
                request.InterimExecutiveBoardCreated,
                request.InterimExecutiveBoardCreatedDetails,
                request.InterimExecutiveBoardCreatedDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}