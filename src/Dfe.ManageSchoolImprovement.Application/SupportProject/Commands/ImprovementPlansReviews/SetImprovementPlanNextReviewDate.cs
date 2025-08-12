using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class SetImprovementPlanNextReviewDate
{
    public record SetImprovementPlanNextReviewDateCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            ImprovementPlanReviewId ImprovementPlanReviewId,
            DateTime? NextReviewDate
    ) : IRequest<bool>;

    public class SetImprovementPlanNextReviewDateCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetImprovementPlanNextReviewDateCommand, bool>
    {
        public async Task<bool> Handle(SetImprovementPlanNextReviewDateCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetImprovementPlanReviewNextReviewDate(
                request.ImprovementPlanId,
                request.ImprovementPlanReviewId,
                request.NextReviewDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
