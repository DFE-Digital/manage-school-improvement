using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class DeleteImprovementPlanObjective
{
    public record DeleteImprovementPlanObjectiveCommand(
        SupportProjectId SupportProjectId,
        ImprovementPlanId ImprovementPlanId,
        ImprovementPlanObjectiveId ImprovementPlanObjectiveId,
        string DeletedBy
    ) : IRequest<bool>;

    public class DeleteImprovementPlanObjectiveCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<DeleteImprovementPlanObjectiveCommand, bool>
    {
        public async Task<bool> Handle(DeleteImprovementPlanObjectiveCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }
            
            supportProject.DeleteImprovementPlanObjective(request.ImprovementPlanId, request.ImprovementPlanObjectiveId, request.DeletedBy);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}