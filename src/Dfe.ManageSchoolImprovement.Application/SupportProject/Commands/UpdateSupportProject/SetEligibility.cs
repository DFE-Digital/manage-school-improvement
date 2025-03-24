using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetEligibilityCommand(
        SupportProjectId SupportProjectId,
        bool? SchoolIsEligible,
        string? SchoolIsNotEligibleNotes
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

            supportProject.SetEligibility(request.SchoolIsEligible, request.SchoolIsNotEligibleNotes);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
