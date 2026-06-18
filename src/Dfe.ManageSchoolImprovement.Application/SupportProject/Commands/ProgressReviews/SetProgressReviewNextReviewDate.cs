using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public record SetProgressReviewNextReviewDateCommand(
    SupportProjectId SupportProjectId,
    ProgressReviewId ProgressReviewId,
    DateTime? NextReviewDate
) : IRequest<bool>;

public class SetProgressReviewNextReviewDate
{
    public class SetProgressReviewNextReviewDateCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetProgressReviewNextReviewDateCommand, bool>
    {
        public async Task<bool> Handle(SetProgressReviewNextReviewDateCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetProgressReviewNextReviewDate(request.ProgressReviewId, request.NextReviewDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}