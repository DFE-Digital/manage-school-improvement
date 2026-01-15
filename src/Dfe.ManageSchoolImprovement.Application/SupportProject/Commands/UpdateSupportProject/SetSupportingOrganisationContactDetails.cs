using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;



public record SetSupportingOrganisationContactDetailsCommand(
    SupportProjectId SupportProjectId,
    string? supportingOrganisationContactName,
    string? supportingOrganisationContactEmail
) : IRequest<bool>;

public class SetSupportingOrganisationContactDetails
{
    public class SetSupportingOrganisationContactDetailsHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSupportingOrganisationContactDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetSupportingOrganisationContactDetailsCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetSupportingOrganisationContactDetails(request.supportingOrganisationContactName,
                request.supportingOrganisationContactEmail);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
