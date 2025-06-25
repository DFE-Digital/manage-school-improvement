using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetIndicativeFundingBandAndImprovementPlanTemplateDetails
{
    public record SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand(
        SupportProjectId SupportProjectId,
        bool? IndicativeFundingBandCalculated,
        string? IndicativeFundingBand,
        bool? ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
        DateTime? DateTemplatesAndIndicativeFundingBandSent
    ) : IRequest<bool>;

    public class SetIndicativeFundingBandAndImprovementPlanTemplateDetailsHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetIndicativeFundingBandAndImprovementPlanTemplateDetails(
                request.IndicativeFundingBandCalculated,
                request.IndicativeFundingBand,
                request.ImprovementPlanAndExpenditurePlanWithIndicativeFundingBandSentToSupportingOrganisationAndSchoolsResponsibleBody,
                request.DateTemplatesAndIndicativeFundingBandSent);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
