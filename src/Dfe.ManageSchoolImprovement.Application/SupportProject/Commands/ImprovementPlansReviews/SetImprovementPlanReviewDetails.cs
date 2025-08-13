using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class SetImprovementPlanReviewDetails
{
    public record SetImprovementPlanReviewDetailsCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            ImprovementPlanReviewId ImprovementPlanReviewId,
            string Reviewer,
            DateTime ReviewDate
    ) : IRequest<bool>;

    public class SetImprovementPlanReviewDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetImprovementPlanReviewDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetImprovementPlanReviewDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetImprovementPlanReviewDetails(
                request.ImprovementPlanId,
                request.ImprovementPlanReviewId,
                request.Reviewer,
                request.ReviewDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
