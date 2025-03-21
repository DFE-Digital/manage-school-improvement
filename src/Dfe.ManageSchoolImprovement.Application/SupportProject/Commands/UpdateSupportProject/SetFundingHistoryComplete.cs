using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetFundingHistoryComplete
{
    public record SetFundingHistoryCompleteCommand(
            SupportProjectId SupportProjectId,
            bool? IsComplete
    ) : IRequest<bool>;

    public class SetFundingHistoryCompleteCommandHandler(ISupportProjectRepository supportProjectRepository, IDateTimeProvider _dateTimeProvider)
        : IRequestHandler<SetFundingHistoryCompleteCommand, bool>
    {
        public async Task<bool> Handle(SetFundingHistoryCompleteCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                throw new ArgumentException($"Support project with id {request.SupportProjectId} not found");
            }

            supportProject.SetFundingHistoryComplete(request.IsComplete);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
