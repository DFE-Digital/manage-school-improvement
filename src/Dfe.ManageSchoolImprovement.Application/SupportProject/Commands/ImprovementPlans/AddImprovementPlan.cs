using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class AddImprovementPlan
{
    public record AddImprovementPlanCommand(
            SupportProjectId SupportProjectId
    ) : IRequest<ImprovementPlanId>;

    public class AddImprovementPlanCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<AddImprovementPlanCommand, ImprovementPlanId>
    {
        public async Task<ImprovementPlanId> Handle(AddImprovementPlanCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());

            supportProject.AddImprovementPlan(improvementPlanId, request.SupportProjectId);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return improvementPlanId;
        }
    }
}
