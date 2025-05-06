using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;


public record SetReviewTheImprovementPlanCommand(
    SupportProjectId SupportProjectId,
    DateTime? ImprovementPlanReceivedDate,
    bool? ReviewImprovementPlanWithTeam,
    bool? SendImprovementPlanToRiseGrantTeam,
    bool? ConfirmPlanClearedByRiseGrantTeam
) : IRequest<bool>;

public class SetReviewTheImprovementPlanCommandHandler(ISupportProjectRepository supportProjectRepository)
    : IRequestHandler<SetReviewTheImprovementPlanCommand, bool>
{
    public async Task<bool> Handle(SetReviewTheImprovementPlanCommand request,
        CancellationToken cancellationToken)
    {
        var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

        if (supportProject is null)
        {
            return false;
        }

        supportProject.SetReviewTheImprovementPlan(request.ImprovementPlanReceivedDate,request.ReviewImprovementPlanWithTeam, request.SendImprovementPlanToRiseGrantTeam, request.ConfirmPlanClearedByRiseGrantTeam);

        await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

        return true;
    }
}
