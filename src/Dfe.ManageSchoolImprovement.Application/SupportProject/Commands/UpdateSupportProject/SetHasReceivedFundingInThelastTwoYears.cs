using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetHasReceivedFundingInThelastTwoYearsCommand(
        SupportProjectId SupportProjectId,
        bool? HasReceivedFundingInThelastTwoYearsCommand
    ) : IRequest<bool>;

    public class SetHasReceivedFundingInThelastTwoYearsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetHasReceivedFundingInThelastTwoYearsCommand, bool>
    {
        public async Task<bool> Handle(SetHasReceivedFundingInThelastTwoYearsCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetHasReceivedFundingInThelastTwoYearsCommand(request.HasReceivedFundingInThelastTwoYearsCommand);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
