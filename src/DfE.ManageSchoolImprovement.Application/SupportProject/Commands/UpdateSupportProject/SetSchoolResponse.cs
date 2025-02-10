using DfE.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using DfE.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace DfE.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetSchoolResponseCommand(
        SupportProjectId SupportProjectId,
        DateTime? SchoolResponseDate,
        bool? HasAcceeptedTargetedSupport,
        bool? HasSavedSchoolResponseinSharePoint
    ) : IRequest<bool>;

    public class SetSchoolResponseCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSchoolResponseCommand, bool>
    {
        public async Task<bool> Handle(SetSchoolResponseCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetSchoolResponse(request.SchoolResponseDate, request.HasAcceeptedTargetedSupport, request.HasSavedSchoolResponseinSharePoint);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
