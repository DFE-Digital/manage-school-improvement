using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetInitialContactTheResponsibleBodyDetailsCommand(
    SupportProjectId SupportProjectId,
    bool? InitialContactResponsibleBody,
    DateTime? ResponsibleBodyInitialContactDate
) : IRequest<bool>;

public class SetInitialContactTheResponsibleBodyDetails
{
    public class SetInitialContactTheResponsibleBodyDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetInitialContactTheResponsibleBodyDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetInitialContactTheResponsibleBodyDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }
            
            supportProject.SetInitialContactTheResponsibleBodyDetails(request.InitialContactResponsibleBody,request.ResponsibleBodyInitialContactDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
