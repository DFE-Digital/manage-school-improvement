using DfE.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using DfE.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace DfE.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetRecordImprovementPlanDecisionCommand(
        SupportProjectId SupportProjectId,
        DateTime? RegionalDirectorImprovementPlanDecisionDate,
        bool? HasApprovedImprovementPlanDecision,
        string? DisapprovingImprovementPlanDecisionNotes
    ) : IRequest<bool>;

    public class SetRecordImprovementPlanDecisionCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetRecordImprovementPlanDecisionCommand, bool>
    {
        public async Task<bool> Handle(SetRecordImprovementPlanDecisionCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetRecordImprovementPlanDecision(request.RegionalDirectorImprovementPlanDecisionDate, request.HasApprovedImprovementPlanDecision, request.DisapprovingImprovementPlanDecisionNotes);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
