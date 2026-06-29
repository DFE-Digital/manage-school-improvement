using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public record SetProgressDetailsCommand(
    SupportProjectId SupportProjectId,
    ProgressReviewId ProgressReviewId,
    string NextSteps,
    string? AdditionalDetails
) : IRequest<bool>;

public class SetProgressDetails
{
    public class SetProgressDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetProgressDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetProgressDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetProgressDetails(request.ProgressReviewId, request.NextSteps, request.AdditionalDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}