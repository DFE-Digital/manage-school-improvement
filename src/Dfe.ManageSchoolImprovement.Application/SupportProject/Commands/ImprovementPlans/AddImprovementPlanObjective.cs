using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class AddImprovementPlanObjective
{
    public record AddImprovementPlanObjectiveCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            string areaOfImprovement,
            string details
    ) : IRequest<ImprovementPlanObjectiveId>;

    public class AddImprovementPlanObjectiveCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<AddImprovementPlanObjectiveCommand, ImprovementPlanObjectiveId>
    {
        public async Task<ImprovementPlanObjectiveId> Handle(AddImprovementPlanObjectiveCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            var improvementPlanObjectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            supportProject.AddImprovementPlanObjective(improvementPlanObjectiveId, request.ImprovementPlanId, request.SupportProjectId, request.areaOfImprovement, request.details);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return improvementPlanObjectiveId;
        }
    }
}
