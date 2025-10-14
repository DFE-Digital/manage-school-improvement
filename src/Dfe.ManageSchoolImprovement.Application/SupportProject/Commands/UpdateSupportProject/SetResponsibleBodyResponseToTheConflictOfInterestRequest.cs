using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetResponsibleBodyResponseToTheConflictOfInterestRequestCommand(
        SupportProjectId SupportProjectId,
        DateTime? ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate,
        bool? ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint
    ) : IRequest<bool>;

    public class SetResponsibleBodyResponseToTheConflictOfInterestRequestCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetResponsibleBodyResponseToTheConflictOfInterestRequestCommand, bool>
    {
        public async Task<bool> Handle(SetResponsibleBodyResponseToTheConflictOfInterestRequestCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetResponsibleBodyResponseToTheConflictOfInterestRequest(request.ResponsibleBodyResponseToTheConflictOfInterestRequestReceivedDate, request.ResponsibleBodyResponseToTheConflictOfInterestRequestSavedInSharePoint);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
