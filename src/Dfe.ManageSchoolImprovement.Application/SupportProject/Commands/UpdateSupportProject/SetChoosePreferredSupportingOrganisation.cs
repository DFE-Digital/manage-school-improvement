using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;


public record SetChoosePreferredSupportingOrganisationCommand(
    SupportProjectId SupportProjectId,
    string? OrganisationName,
    string? IdNumber,
    string? OrganisationType,
    DateTime? DateSupportOrganisationChosen,
    bool? AssessmentToolTwoCompleted,
    string? Address,
    string? ContactName,
    string? ContactEmail,
    string? ContactPhone,
    DateTime? DateContactDetailsAdded
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

            supportProject.SetChoosePreferredSupportOrganisation(new SupportingOrganisationDetails()
                {
                    DateSupportOrganisationChosen = request.DateSupportOrganisationChosen,
                    SupportOrganisationName = request.OrganisationName,
                    SupportOrganisationIdNumber = request.IdNumber,
                    SupportOrganisationType = request.OrganisationType,
                    AssessmentToolTwoCompleted = request.AssessmentToolTwoCompleted,
                    SupportOrganisationAddress = request.Address,
                    SupportOrganisationContactName = request.ContactName,
                    SupportOrganisationContactEmailAddress = request.ContactEmail,
                    SupportOrganisationContactPhone = request.ContactPhone,
                    DateSupportingOrganisationContactDetailsAdded = request.DateContactDetailsAdded
                });

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
