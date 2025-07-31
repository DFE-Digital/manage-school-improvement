using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetAdviserDetails
{
    public record SetAdviserDetailsCommand(
        SupportProjectId SupportProjectId,
        DateTime? DateAdviserAllocated,
        string? AdviserEmailAddress,
        string? AdviserFullName
    ) : IRequest<bool>;

    public class SetAdviserDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetAdviserDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetAdviserDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetAdviserDetails(request.AdviserEmailAddress, request.DateAdviserAllocated, request.AdviserFullName);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
