using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class SetImprovementPlanObjectiveDetails
{
    public record SetImprovementPlanObjectiveDetailsCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            ImprovementPlanObjectiveId ImprovementPlanObjectiveId,
            string details
    ) : IRequest<bool>;

    public class SetImprovementPlanObjectiveDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetImprovementPlanObjectiveDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetImprovementPlanObjectiveDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetImprovementPlanObjectiveDetails(request.ImprovementPlanObjectiveId, request.ImprovementPlanId, request.details);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
