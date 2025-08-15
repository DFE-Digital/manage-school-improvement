using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class SetImprovementPlanObjectiveProgressDetails
{
    public record SetImprovementPlanObjectiveProgressDetailsCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            ImprovementPlanReviewId ImprovementPlanReviewId,
            ImprovementPlanObjectiveProgressId ImprovementPlanObjectiveProgressId,
            string progressStatus,
            string progressDetails
    ) : IRequest<bool>;

    public class SetImprovementPlanObjectiveProgressDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetImprovementPlanObjectiveProgressDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetImprovementPlanObjectiveProgressDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetImprovementPlanObjectiveProgressDetails(
                request.ImprovementPlanId,
                request.ImprovementPlanReviewId,
                request.ImprovementPlanObjectiveProgressId,
                request.progressStatus,
                request.progressDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
