using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlansReviews;

public class DeleteImprovementPlanReview
{
    public record DeleteImprovementPlanReviewCommand(
        SupportProjectId SupportProjectId,
        ImprovementPlanId ImprovementPlanId,
        ImprovementPlanReviewId ImprovementPlanReviewId,
        string DeletedBy
    ) : IRequest<bool>;

    public class DeleteImprovementPlanReviewCommandHandler(
        ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<DeleteImprovementPlanReviewCommand, bool>
    {
        public async Task<bool> Handle(DeleteImprovementPlanReviewCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject =
                await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.DeleteImprovementPlanReview(request.ImprovementPlanId,
                request.ImprovementPlanReviewId, request.DeletedBy);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}