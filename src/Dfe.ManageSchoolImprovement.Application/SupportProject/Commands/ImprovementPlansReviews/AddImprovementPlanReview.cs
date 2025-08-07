using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlansReviews;

public class AddImprovementPlanReview
{
    public record AddImprovementPlanReviewCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            string Reviewer,
            DateTime ReviewDate
    ) : IRequest<ImprovementPlanReviewId>;

    public class AddImprovementPlanReviewCommandCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<AddImprovementPlanReviewCommand, ImprovementPlanReviewId>
    {
        public async Task<ImprovementPlanReviewId> Handle(AddImprovementPlanReviewCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            var improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());

            supportProject.AddImprovementPlanReview(improvementPlanReviewId, request.ImprovementPlanId, request.Reviewer, request.ReviewDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return improvementPlanReviewId;
        }
    }
}
