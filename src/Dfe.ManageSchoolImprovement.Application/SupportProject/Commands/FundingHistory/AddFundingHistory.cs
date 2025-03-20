using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory;

public class AddFundingHistory
{
    public record AddFundingHistoryCommand(
            SupportProjectId SupportProjectId,
            string FundingType,
            double FundingAmount,
            string FinancialYear,
            int FundingRounds,
            string Comments
    ) : IRequest<FundingHistoryId>;

    public class AddFundingHistoryCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<AddFundingHistoryCommand, FundingHistoryId>
    {
        public async Task<FundingHistoryId> Handle(AddFundingHistoryCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            var fundingHistoryId = new FundingHistoryId(Guid.NewGuid());

            supportProject.AddFundingHistory(fundingHistoryId, request.SupportProjectId, request.FundingType, request.FundingAmount, request.FinancialYear, request.FundingRounds, request.Comments);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return fundingHistoryId;
        }
    }
}
