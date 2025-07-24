using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class SetImprovementPlanObjectivesComplete
{
    public record SetImprovementPlanObjectivesCompleteCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            bool ObjectivesSectionComplete
    ) : IRequest<bool>;

    public class SetImprovementPlanObjectivesCompleteCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetImprovementPlanObjectivesCompleteCommand, bool>
    {
        public async Task<bool> Handle(SetImprovementPlanObjectivesCompleteCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetImprovementPlanObjectivesComplete(request.ImprovementPlanId, request.ObjectivesSectionComplete);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
