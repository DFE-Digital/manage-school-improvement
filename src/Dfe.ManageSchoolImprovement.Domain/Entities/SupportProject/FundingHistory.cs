﻿using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class FundingHistory : IEntity<FundingHistoryId>
    {
        public FundingHistory(
            FundingHistoryId id,
            SupportProjectId supportProjectId,
            string fundingType,
            decimal fundingAmount,
            string financialYear,
            int fundingRounds,
            string comments
            )
        {
            Id = id;
            SupportProjectId = supportProjectId;
            FundingType = fundingType;
            FundingAmount = fundingAmount;
            FinancialYear = financialYear;
            FundingRounds = fundingRounds;
            Comments = comments;
        }
        public FundingHistoryId? Id { get; private set; }
        public int ReadableId { get; }
        public SupportProjectId SupportProjectId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }

        public string FundingType { get; private set; }
        public decimal FundingAmount { get; private set; }
        public string FinancialYear { get; private set; }
        public int FundingRounds { get; private set; }
        public string? Comments { get; private set; }

        public void SetValues(string fundingType, decimal fundingAmount, string financialYear, int fundingRounds, string comments)
        {
            FundingType = fundingType;
            FundingAmount = fundingAmount;
            FinancialYear = financialYear;
            FundingRounds = fundingRounds;
            Comments = comments;
        }
    }
}
