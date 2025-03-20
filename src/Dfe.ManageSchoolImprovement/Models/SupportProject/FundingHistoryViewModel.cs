namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class FundingHistoryViewModel
    {
        public Guid Id { get; set; }
        public int SupportProjectId { get; set; }
        public string FundingType { get; set; }
        public double FundingAmount { get; set; }
        public string FinancialYear { get; set; }
        public int FundingRounds { get; set; }
        public string Comments { get; set; }

    }
}
