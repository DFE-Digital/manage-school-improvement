namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class FundingHistoryViewModel
    {
        public Guid Id { get; set; }
        public int SupportProjectId { get; set; }
        public int ReadableId { get; set; }
        public string FundingType { get; set; }
        public decimal FundingAmount { get; set; }
        public string FinancialYear { get; set; }
        public int FundingRounds { get; set; }
        public string Comments { get; set; }

    }
}
