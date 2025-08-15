using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class FundingHistoryViewModel
    {
        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public int SupportProjectId { get; set; }
        public string FundingType { get; set; } = string.Empty;
        public decimal FundingAmount { get; set; }
        public string FinancialYear { get; set; } = string.Empty;
        public int FundingRounds { get; set; }
        public string Comments { get; set; } = string.Empty;

        public static FundingHistoryViewModel Create(FundingHistoryDto dto)
        {
            return new FundingHistoryViewModel
            {
                Id = dto.id,
                ReadableId = dto.readableId,
                SupportProjectId = dto.supportProjectId,
                FundingType = dto.fundingType,
                FundingAmount = dto.fundingAmount,
                FinancialYear = dto.financialYear,
                FundingRounds = dto.fundingRounds,
                Comments = dto.comments
            };
        }
    }
}
