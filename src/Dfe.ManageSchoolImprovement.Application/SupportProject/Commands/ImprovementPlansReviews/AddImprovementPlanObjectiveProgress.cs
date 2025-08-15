using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class AddImprovementPlanObjectiveProgress
{
    public record AddImprovementPlanObjectiveProgressCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            ImprovementPlanReviewId ImprovementPlanReviewId,
            ImprovementPlanObjectiveId ImprovementPlanObjectiveId,
            string progressStatus,
            string progressDetails
    ) : IRequest<ImprovementPlanObjectiveProgressId>;

    public class AddImprovementPlanObjectiveProgressCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<AddImprovementPlanObjectiveProgressCommand, ImprovementPlanObjectiveProgressId>
    {
        public async Task<ImprovementPlanObjectiveProgressId> Handle(AddImprovementPlanObjectiveProgressCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            var improvementPlanObjectiveProgressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());

            supportProject.AddImprovementPlanObjectiveProgress(
                improvementPlanObjectiveProgressId,
                request.ImprovementPlanId,
                request.ImprovementPlanReviewId,
                request.ImprovementPlanObjectiveId,
                request.progressStatus,
                request.progressDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return improvementPlanObjectiveProgressId;
        }
    }
}
