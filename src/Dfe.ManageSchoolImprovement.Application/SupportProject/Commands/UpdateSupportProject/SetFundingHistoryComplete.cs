using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetFundingHistoryComplete
{
    public record SetFundingHistoryCompleteCommand(
            SupportProjectId SupportProjectId,
            bool? IsComplete
    ) : IRequest<bool>;

    public class SetFundingHistoryCompleteCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetFundingHistoryCompleteCommand, bool>
    {
        public async Task<bool> Handle(SetFundingHistoryCompleteCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetFundingHistoryComplete(request.IsComplete);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
