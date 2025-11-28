using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;


public record SetChoosePreferredSupportingOrganisationCommand(
    SupportProjectId SupportProjectId,
    string? OrganisationName,
    string? IdNumber,
    string? OrganisationType,
    DateTime? DateSupportOrganisationChosen,
    bool? AssessmentToolTwoCompleted,
    string? Address
) : IRequest<bool>;
public class SetChoosePreferredSupportingOrganisation
{
    public class SetChoosePreferredSupportingOrganisationHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetChoosePreferredSupportingOrganisationCommand, bool>
    {
        public async Task<bool> Handle(SetChoosePreferredSupportingOrganisationCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetChoosePreferredSupportOrganisation(request.DateSupportOrganisationChosen,
                request.OrganisationName,
                request.IdNumber,
                request.OrganisationType,
                request.AssessmentToolTwoCompleted,
                request.Address);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
