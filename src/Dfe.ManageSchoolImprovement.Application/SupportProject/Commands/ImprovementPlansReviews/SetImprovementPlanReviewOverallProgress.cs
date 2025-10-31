using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;

public class SetImprovementPlanReviewOverallProgress
{
    public record SetImprovementPlanReviewOverallProgressCommand(
            SupportProjectId SupportProjectId,
            ImprovementPlanId ImprovementPlanId,
            ImprovementPlanReviewId ImprovementPlanReviewId,
            // string howIsTheSchoolProgressingOverall,
            string overallProgressDetails
    ) : IRequest<bool>;

    public class SetImprovementPlanReviewOverallProgressCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetImprovementPlanReviewOverallProgressCommand, bool>
    {
        public async Task<bool> Handle(SetImprovementPlanReviewOverallProgressCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new KeyNotFoundException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetOverallProgress(
                request.ImprovementPlanId,
                request.ImprovementPlanReviewId,
                // request.howIsTheSchoolProgressingOverall,
                request.overallProgressDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
