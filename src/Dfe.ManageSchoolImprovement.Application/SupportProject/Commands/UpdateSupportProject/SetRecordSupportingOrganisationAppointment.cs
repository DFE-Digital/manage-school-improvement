using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetRecordSupportingOrganisationAppointmentCommand(
        SupportProjectId SupportProjectId,
        DateTime? RegionalDirectorAppointmentDate,
        bool? HasConfirmedSupportingOrganisationAppointment,
        string? DisapprovingSupportingOrganisationAppointmentNotes
    ) : IRequest<bool>;

    public class SetRecordSupportingOrganisationAppointmentCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetRecordSupportingOrganisationAppointmentCommand, bool>
    {
        public async Task<bool> Handle(SetRecordSupportingOrganisationAppointmentCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetRecordSupportingOrganisationAppointment(request.RegionalDirectorAppointmentDate, request.HasConfirmedSupportingOrganisationAppointment, request.DisapprovingSupportingOrganisationAppointmentNotes);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
