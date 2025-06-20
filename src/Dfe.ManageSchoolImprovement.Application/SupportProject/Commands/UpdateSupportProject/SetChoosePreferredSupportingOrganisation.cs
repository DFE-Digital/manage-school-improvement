using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;


public record SetChoosePreferredSupportingOrganisationCommand(
    SupportProjectId SupportProjectId,
    string? OrganisationName,
    string? IDNumber,
    DateTime? DateSupportOrganisationChosen,
    bool? AssessmentToolTwoCompleted
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
                request.IDNumber,
                request.AssessmentToolTwoCompleted);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
