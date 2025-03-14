using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetDeliveryOfficerCommand(
    SupportProjectId SupportProjectId,
    string AssignedDeliveryOfficerFullName,
    string AssignedDeliveryOfficerEmail
) : IRequest<bool>;


public class SetDeliveryOfficer
{
    
    public class SetDeliveryOfficerCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetDeliveryOfficerCommand,bool>    {

        public async Task<bool> Handle(SetDeliveryOfficerCommand request,
            CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetDeliveryOfficer(request.AssignedDeliveryOfficerFullName, request.AssignedDeliveryOfficerEmail);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);
            
            return true;
        }
    }

}
