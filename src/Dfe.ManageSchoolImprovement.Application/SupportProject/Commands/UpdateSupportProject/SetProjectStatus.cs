using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetProjectStatusCommand(
        SupportProjectId SupportProjectId,
        ProjectStatusValue ProjectStatus,
        DateTime? DateProjectStatusChanged,
        string? ChangedBy,
        string? Details
    ) : IRequest<bool>;

    public class SetProjectStatusCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetProjectStatusCommand, bool>
    {
        public async Task<bool> Handle(SetProjectStatusCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetProjectStatus(request.ProjectStatus, request.DateProjectStatusChanged, request.ChangedBy, request.Details);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}