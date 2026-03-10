using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlansReviews;

public class DeleteImprovementPlanObjectiveProgress
{
    public record DeleteImprovementPlanObjectiveProgressCommand(
        SupportProjectId SupportProjectId,
        ImprovementPlanId ImprovementPlanId,
        ImprovementPlanReviewId ImprovementPlanReviewId,
        ImprovementPlanObjectiveProgressId ImprovementPlanObjectiveProgressId,
        string DeletedBy
    ) : IRequest<bool>;

    public class DeleteImprovementPlanObjectiveProgressCommandHandler(
        ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<DeleteImprovementPlanObjectiveProgressCommand, bool>
    {
        public async Task<bool> Handle(DeleteImprovementPlanObjectiveProgressCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject =
                await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.DeleteImprovementPlanObjectiveProgress(request.ImprovementPlanId,
                request.ImprovementPlanReviewId, request.ImprovementPlanObjectiveProgressId, request.DeletedBy);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}