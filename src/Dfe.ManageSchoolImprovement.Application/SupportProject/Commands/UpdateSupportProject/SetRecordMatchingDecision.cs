using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetRecordMatchingDecisionCommand(
        SupportProjectId SupportProjectId,
        DateTime? RegionalDirectorDecisionDate,
        bool? HasSchoolMatchedWithSupportingOrganisation,
        string? NotMatchingSchoolWithSupportingOrgNotes
    ) : IRequest<bool>;

    public class SetRecordMatchingDecisionCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetRecordMatchingDecisionCommand, bool>
    {
        public async Task<bool> Handle(SetRecordMatchingDecisionCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetRecordMatchingDecision(request.RegionalDirectorDecisionDate, request.HasSchoolMatchedWithSupportingOrganisation, request.NotMatchingSchoolWithSupportingOrgNotes);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
