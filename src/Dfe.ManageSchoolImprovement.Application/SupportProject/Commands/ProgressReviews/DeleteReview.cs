using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public record DeleteReviewCommand(
    SupportProjectId SupportProjectId,
    ProgressReviewId ProgressReviewId
) : IRequest<bool>;

public class DeleteReview
{
    public class DeleteReviewCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<DeleteReviewCommand, bool>
    {
        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.DeleteReview(request.ProgressReviewId, request.SupportProjectId);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}