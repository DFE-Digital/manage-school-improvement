using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ProgressReviews;

public record AddProgressReviewCommand(
    SupportProjectId SupportProjectId,
    string Reviewer,
    DateTime ReviewDate
) : IRequest<ProgressReviewId>;

public class AddProgressReview
{
    public class AddProgressReviewCommandCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<AddProgressReviewCommand, ProgressReviewId>
    {
        public async Task<ProgressReviewId> Handle(AddProgressReviewCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            var progressReviewId = new ProgressReviewId(Guid.NewGuid());

            supportProject.AddProgressReview(progressReviewId, request.SupportProjectId, request.Reviewer, request.ReviewDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return progressReviewId;
        }
    }
}