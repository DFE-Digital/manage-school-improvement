namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record FundingHistoryDto(Guid id,
            int readableId,
            int supportProjectId,
            string fundingType,
            double fundingAmount,
            string financialYear,
            int fundingRounds,
            string comments)
    {
    }
}
