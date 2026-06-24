using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public record DeleteProgressCommand(
    SupportProjectId SupportProjectId,
    ProgressReviewId ProgressReviewId
) : IRequest<bool>;

public class DeleteProgress
{
    public class DeleteProgressCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<DeleteProgressCommand, bool>
    {
        public async Task<bool> Handle(DeleteProgressCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.DeleteProgress(request.ProgressReviewId);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}