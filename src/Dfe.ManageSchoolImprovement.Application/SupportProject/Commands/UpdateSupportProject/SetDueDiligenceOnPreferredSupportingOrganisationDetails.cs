using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetDueDiligenceOnPreferredSupportingOrganisationDetails
{
    public record SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommand(
        SupportProjectId SupportProjectId,
         bool? CheckOrganisationHasCapacityAndWillingToProvideSupport,
         bool? CheckChoiceWithTrustRelationshipManagerOrLaLead,
         bool? DiscussChoiceWithSfso,
         bool? CheckTheOrganisationHasAVendorAccount,
         DateTime? DateDueDiligenceCompleted
    ) : IRequest<bool>;

    public class SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetDueDiligenceOnPreferredSupportingOrganisationDetails(
                request.CheckOrganisationHasCapacityAndWillingToProvideSupport,
                request.CheckChoiceWithTrustRelationshipManagerOrLaLead,
                request.DiscussChoiceWithSfso,
                request.CheckTheOrganisationHasAVendorAccount,
                request.DateDueDiligenceCompleted);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
