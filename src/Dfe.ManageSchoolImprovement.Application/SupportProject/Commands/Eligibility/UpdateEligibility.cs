using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Mediator;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Eligibility
{
    public record UpdateEligibilityCommand(
        SupportProjectId SupportProjectId,
        SupportProjectEligibilityStatus? SchoolIsEligible,
        DateTime? DateEligibilityChanged,
        string? EligibilityChangedBy,
        DateTime? DateSupportIsDueToEnd,
        string? EligibilityChangedDetails
    ) : IRequest<bool>;

    public class UpdateEligibilityCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<UpdateEligibilityCommand, bool>
    {
        public async ValueTask<bool> Handle(UpdateEligibilityCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.UpdateEligibility(request.SchoolIsEligible, request.DateEligibilityChanged, request.DateSupportIsDueToEnd, request.EligibilityChangedBy, request.EligibilityChangedDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}