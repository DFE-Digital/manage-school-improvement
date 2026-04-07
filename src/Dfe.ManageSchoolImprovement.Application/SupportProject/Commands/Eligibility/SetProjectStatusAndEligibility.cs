using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetProjectStatusAndEligibilityCommand(
        SupportProjectId SupportProjectId,
        ProjectStatusValue ProjectStatus,
        SupportProjectEligibilityStatus EligibilityStatus,
        DateTime? DateChanged,
        string? ChangedBy,
        string? Details,
        DateTime? DateSupportIsDueToEnd
    ) : IRequest<bool>;

    public class SetProjectStatusAndEligibilityCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetProjectStatusAndEligibilityCommand, bool>
    {
        public async Task<bool> Handle(SetProjectStatusAndEligibilityCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetProjectStatus(request.ProjectStatus, request.DateChanged, request.ChangedBy, request.Details);
            supportProject.UpdateEligibility(request.EligibilityStatus, request.DateChanged, request.DateSupportIsDueToEnd, request.ChangedBy, request.Details);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
