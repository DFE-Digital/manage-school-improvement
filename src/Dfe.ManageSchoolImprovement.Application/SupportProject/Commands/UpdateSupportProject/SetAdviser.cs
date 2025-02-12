using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetAdviserCommand(
    SupportProjectId SupportProjectId,
    string AssignedAdviserFullName,
    string AssignedAdviserEmail
) : IRequest<bool>;


public class SetAdviser
{
    
    public class SetAdviserCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetAdviserCommand,bool>    {

        public async Task<bool> Handle(SetAdviserCommand request,
            CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetAdviser(request.AssignedAdviserFullName, request.AssignedAdviserEmail);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);
            
            return true;
        }
    }

}
