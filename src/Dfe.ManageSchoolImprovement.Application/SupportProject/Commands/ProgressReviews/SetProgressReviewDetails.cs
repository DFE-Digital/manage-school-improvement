using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ProgressReviews;

public record SetProgressReviewDetailsCommand(
    ProgressReviewId ProgressReviewId,
    SupportProjectId SupportProjectId,
    string Reviewer,
    DateTime ReviewDate
) : IRequest<bool>;

public class SetProgressReviewDetails
{
    public class SetProgressReviewDetailsCommandCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetProgressReviewDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetProgressReviewDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }
            
            supportProject.SetProgressReviewDetails(request.ProgressReviewId, request.Reviewer, request.ReviewDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}