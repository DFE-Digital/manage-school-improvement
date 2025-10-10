using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetAdviserConflictOfInterestDetails
{
    public record SetAdviserConflictOfInterestDetailsCommand(
        SupportProjectId SupportProjectId,
        bool? ReviewAdvisersConflictOfInterestForm,
        DateTime? DateConflictOfInterestDeclarationChecked
    ) : IRequest<bool>;

    public class SetAdviserConflictOfInterestDetailsHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetAdviserConflictOfInterestDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetAdviserConflictOfInterestDetailsCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject =
                await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetAdviserConflictOfInterestDetails(
                request.ReviewAdvisersConflictOfInterestForm,
                request.DateConflictOfInterestDeclarationChecked);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}