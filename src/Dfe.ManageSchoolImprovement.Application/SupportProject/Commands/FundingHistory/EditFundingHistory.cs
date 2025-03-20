using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;
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
    ) : IRequest<FundingHistoryId>;

    public class EditFundingHistoryCommandHandler(ISupportProjectRepository supportProjectRepository, IDateTimeProvider _dateTimeProvider)
        : IRequestHandler<EditFundingHistoryCommand, FundingHistoryId>
    {
        public async Task<FundingHistoryId> Handle(EditFundingHistoryCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);
            if (supportProject == null)
            {
                throw new ArgumentException($"Support project with id {request.SupportProjectId} not found");
            }
            supportProject.EditFundingHistory(request.Id, request.FundingType, request.FundingAmount, request.FinancialYear, request.FundingRounds, request.Comments);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return request.Id;
        }
    }
}
