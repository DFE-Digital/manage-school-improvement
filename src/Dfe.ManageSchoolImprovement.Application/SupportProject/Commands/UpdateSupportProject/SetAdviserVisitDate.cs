using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetAdviserVisitDateCommand(
    SupportProjectId SupportProjectId,
    DateTime? AdviserVisitDate,
    bool? GiveTheAdviserTheNoteOfVisitTemplate
) : IRequest<bool>;

public class SetAdviserVisitDate
{
    public class SetAdviserVisitDateCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetAdviserVisitDateCommand, bool>
    {
        public async Task<bool> Handle(SetAdviserVisitDateCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }
            
            supportProject.SetAdviserVisitDate(request.AdviserVisitDate, request.GiveTheAdviserTheNoteOfVisitTemplate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
