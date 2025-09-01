using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetSendFormalNotificationCommand(
    SupportProjectId SupportProjectId,
    bool? UseEnrolmentLetterTemplateToDraftEmail,
    bool? AttachTargetedInterventionInformationSheet,
    bool? AddRecipientsForFormalNotification,
    bool? FormalNotificationSent,
    DateTime? DateFormalNotificationSent
) : IRequest<bool>;

public class SetSendFormalNotification
{
    public class SetSendFormalNotificationCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSendFormalNotificationCommand, bool>
    {
        public async Task<bool> Handle(SetSendFormalNotificationCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject =
                await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetSendTheFormalNotification(request.UseEnrolmentLetterTemplateToDraftEmail,
                request.AttachTargetedInterventionInformationSheet, 
                request.AddRecipientsForFormalNotification,
                request.FormalNotificationSent,
                request.DateFormalNotificationSent);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}