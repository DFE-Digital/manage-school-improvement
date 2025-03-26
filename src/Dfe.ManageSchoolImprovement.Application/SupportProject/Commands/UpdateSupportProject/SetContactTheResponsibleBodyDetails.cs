using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetContactTheResponsibleBodyDetailsCommand(
    SupportProjectId SupportProjectId,
    bool? discussTheBestApproach,
    bool? emailTheResponsibleBody,
    DateTime? responsibleBodyContactedDate
) : IRequest<bool>;

public class SetContactTheResponsibleBodyDetails
{
    public class SetContactTheResponsibleBodyDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetContactTheResponsibleBodyDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetContactTheResponsibleBodyDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }
            
            supportProject.SetContactTheResponsibleBodyDetails(request.discussTheBestApproach, request.emailTheResponsibleBody,request.responsibleBodyContactedDate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
