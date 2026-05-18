using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public record SetCurrentDeliveryMilestoneCommand(
    SupportProjectId SupportProjectId,
    Milestone CurrentDeliveryMilestone
) : IRequest<bool>;


public class SetCurrentDeliveryMilestone
{
    
    public class SetCurrentDeliveryMilestoneCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetCurrentDeliveryMilestoneCommand,bool>    {

        public async Task<bool> Handle(SetCurrentDeliveryMilestoneCommand request,
            CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetCurrentDeliveryMilestone(request.CurrentDeliveryMilestone);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);
            
            return true;
        }
    }

}