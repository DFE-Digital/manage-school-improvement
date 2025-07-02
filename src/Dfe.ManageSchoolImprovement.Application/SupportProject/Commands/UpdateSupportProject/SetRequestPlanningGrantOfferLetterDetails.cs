using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetRequestPlanningGrantOfferLetterDetails
{
    public record SetRequestPlanningGrantOfferLetterDetailsCommand(
        SupportProjectId SupportProjectId,
        DateTime? DateGrantTeamContacted,
        bool? IncludeContactDetails,
        bool? ConfirmAmountOfFunding,
        bool? CopyInRegionalDirector,
        bool? EmailRiseGrantTeam
    ) : IRequest<bool>;

    public class SetRequestPlanningGrantOfferLetterDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetRequestPlanningGrantOfferLetterDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetRequestPlanningGrantOfferLetterDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetRequestPlanningGrantOfferLetterDetails(
                request.DateGrantTeamContacted, 
                request.IncludeContactDetails, 
                request.ConfirmAmountOfFunding, 
                request.CopyInRegionalDirector, 
                request.EmailRiseGrantTeam);

            await supportProjectRepository.UpdateAsync(supportProject, CancellationToken.None);

            return true;
        }
    }
}
