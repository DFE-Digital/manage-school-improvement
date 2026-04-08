using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Eligibility
{
    public record SetEligibilityCommand(
        SupportProjectId SupportProjectId,
        SupportProjectEligibilityStatus? SchoolIsEligible,
        DateTime? DateEligibilityChanged,
        string? SchoolIsNotEligibleNotes,
        bool EligibilityComplete = false
    ) : IRequest<bool>;

    public class SetEligibilityCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetEligibilityCommand, bool>
    {
        public async Task<bool> Handle(SetEligibilityCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }
            
            supportProject.SetEligibility(request.SchoolIsEligible, request.DateEligibilityChanged, request.SchoolIsNotEligibleNotes, request.EligibilityComplete);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
