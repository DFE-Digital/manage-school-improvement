using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Mediator;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetAdviserConflictOfInterestDetails
{
    public record SetAdviserConflictOfInterestDetailsCommand(
        SupportProjectId SupportProjectId,
        bool? ReviewAdvisersConflictOfInterestForm,
        DateTime? DateConflictOfInterestDeclarationChecked,
        bool? AdviserCanBeSet
    ) : IRequest<bool>;

    public class SetAdviserConflictOfInterestDetailsHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetAdviserConflictOfInterestDetailsCommand, bool>
    {
        public async ValueTask<bool> Handle(SetAdviserConflictOfInterestDetailsCommand request,
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
                request.DateConflictOfInterestDeclarationChecked,
                request.AdviserCanBeSet);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}