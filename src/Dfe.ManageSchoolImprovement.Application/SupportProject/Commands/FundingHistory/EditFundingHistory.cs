using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory;

public class EditFundingHistory
{
    public record EditFundingHistoryCommand(
            FundingHistoryId Id,
            SupportProjectId SupportProjectId,
            string FundingType,
            double FundingAmount,
            string FinancialYear,
            int FundingRounds,
            string Comments
    ) : IRequest<bool>;

    public class EditFundingHistoryCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<EditFundingHistoryCommand, bool>
    {
        public async Task<bool> Handle(EditFundingHistoryCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject == null)
            {
                return false;
            }

            supportProject.EditFundingHistory(request.Id, request.FundingType, request.FundingAmount, request.FinancialYear, request.FundingRounds, request.Comments);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
