using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Mediator;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetConfirmPlanningGrantOfferLetterSentCommand(
    SupportProjectId SupportProjectId,
    DateTime? ConfirmPlanningGrantOfferLetterSentDate
) : IRequest<bool>;

public class SetConfirmPlanningGrantOfferLetterSentCommandHandler(ISupportProjectRepository supportProjectRepository)
    : IRequestHandler<SetConfirmPlanningGrantOfferLetterSentCommand, bool>
{
    public async ValueTask<bool> Handle(SetConfirmPlanningGrantOfferLetterSentCommand request,
        CancellationToken cancellationToken)
    {
        var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

        if (supportProject is null)
        {
            return false;
        }

        supportProject.SetConfirmPlanningGrantOfferLetterDate(request.ConfirmPlanningGrantOfferLetterSentDate);

        await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

        return true;
    }
}
