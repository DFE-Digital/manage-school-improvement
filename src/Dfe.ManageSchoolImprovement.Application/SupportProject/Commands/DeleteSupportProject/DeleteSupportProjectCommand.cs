using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.DeleteSupportProject;

public record DeleteSupportProjectCommand(
    string SchoolUrn
) : IRequest<bool>;

public class DeleteSupportProjectCommandHandler(ISupportProjectRepository supportProjectRepository)
    : IRequestHandler<DeleteSupportProjectCommand, bool>
{
    public async Task<bool> Handle(DeleteSupportProjectCommand request, CancellationToken cancellationToken)
    {
        // Find the support project by school URN
        var supportProject = await supportProjectRepository.GetSupportProjectByUrn(request.SchoolUrn, cancellationToken);

        if (supportProject is null)
        {
            return false;
        }

        // This will perform a hard delete of the support project and all related data
        // due to cascade delete rules in the database
        await supportProjectRepository.RemoveAsync(supportProject, cancellationToken);
        
        return true;
    }
} 
