using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.DeleteSupportProject;

public record DeleteSupportProjectCommand(
    SupportProjectId SupportProjectId
) : IRequest<bool>;

public class DeleteSupportProjectCommandHandler(ISupportProjectRepository supportProjectRepository)
    : IRequestHandler<DeleteSupportProjectCommand, bool>
{
    public async Task<bool> Handle(DeleteSupportProjectCommand request, CancellationToken cancellationToken)
    {
        // Use the repository method that ignores query filters to find even soft-deleted entities
        var supportProject = await supportProjectRepository.GetSupportProjectByIdIgnoringFilters(request.SupportProjectId, cancellationToken);

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
