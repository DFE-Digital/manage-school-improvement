using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class EngagementConcern : IEntity<EngagementConcernId>
    {
        private EngagementConcern() {}
        public EngagementConcern(EngagementConcernId id, SupportProjectId supportProjectId,
            EngagementConcernDetails engagementConcernDetails)
        {
            Id = id;
            SupportProjectId = supportProjectId;
            EngagementConcernDetails = engagementConcernDetails.Details;
            EngagementConcernSummary = engagementConcernDetails.Summary;
            EngagementConcernRaisedDate = engagementConcernDetails.RaisedDate;
            EngagementConcernResolved = engagementConcernDetails.Resolved;
            EngagementConcernResolvedDetails = engagementConcernDetails.ResolvedDetails;
            EngagementConcernResolvedDate = engagementConcernDetails.ResolvedDate;
        }

        public EngagementConcernId Id { get; }
        public int ReadableId { get; }
        public SupportProjectId SupportProjectId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? EngagementConcernDetails { get; set; }
        public string? EngagementConcernSummary { get; set; }
        public DateTime? EngagementConcernRaisedDate { get; set; }
        public bool? EngagementConcernResolved { get; set; }
        public string? EngagementConcernResolvedDetails { get; set; }
        public DateTime? EngagementConcernResolvedDate { get; set; }
        public bool? EngagementConcernEscalationConfirmStepsTaken { get; set; }
        public string? EngagementConcernEscalationPrimaryReason { get; set; }
        public string? EngagementConcernEscalationDetails { get; set; }
        public DateTime? EngagementConcernEscalationDateOfDecision { get; set; }
        public string? EngagementConcernEscalationWarningNotice { get; set; }

        public void SetEngagementConcernDetails(string? engagementConcernDetails, string? engagementConcernSummary, DateTime? engagementConcernRaisedDate)
        {
            EngagementConcernDetails = engagementConcernDetails;
            EngagementConcernSummary = engagementConcernSummary;
            EngagementConcernRaisedDate = engagementConcernRaisedDate;
        }

        public void SetEngagementConcernResolvedDetails(bool? engagementConcernResolved,
            string? engagementConcernResolvedDetails,
            DateTime? engagementConcernResolvedDate)
        {
            EngagementConcernResolved = engagementConcernResolved;
            EngagementConcernResolvedDetails = engagementConcernResolvedDetails;
            EngagementConcernResolvedDate = engagementConcernResolvedDate;
        }

        public void SetEngagementConcernEscalationDetails(bool? confirmStepsTaken, string? primaryReason,
            string? escalationDetails, DateTime? dateOfDecision, string? warningNotice)
        {
            EngagementConcernEscalationConfirmStepsTaken = confirmStepsTaken;
            EngagementConcernEscalationPrimaryReason = primaryReason;
            EngagementConcernEscalationDetails = escalationDetails;
            EngagementConcernEscalationDateOfDecision = dateOfDecision;
            EngagementConcernEscalationWarningNotice = warningNotice;
        }
    }
}