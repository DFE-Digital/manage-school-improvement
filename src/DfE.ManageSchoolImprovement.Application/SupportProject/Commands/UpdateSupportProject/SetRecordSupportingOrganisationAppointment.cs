using DfE.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using DfE.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace DfE.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetRecordSupportingOrganisationAppointmentCommand(
        SupportProjectId SupportProjectId,
        DateTime? RegionalDirectorAppointmentDate,
        bool? HasConfirmedSupportingOrgnaisationAppointment,
        string? DisapprovingSupportingOrgnaisationAppointmentNotes
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

            supportProject.SetRecordSupportingOrganisationAppointment(request.RegionalDirectorAppointmentDate, request.HasConfirmedSupportingOrgnaisationAppointment, request.DisapprovingSupportingOrgnaisationAppointmentNotes);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
