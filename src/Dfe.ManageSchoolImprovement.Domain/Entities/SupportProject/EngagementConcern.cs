using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class EngagementConcern : IEntity<EngagementConcernId>
    {
        public EngagementConcern(EngagementConcernId id, SupportProjectId supportProjectId, bool? engagementConcernRecorded, string? engagementConcernDetails, DateTime? engagementConcernRaisedDate)
        {
            Id = id;
            SupportProjectId = supportProjectId;
            EngagementConcernRecorded = engagementConcernRecorded;
            EngagementConcernDetails = engagementConcernDetails;
            EngagementConcernRaisedDate = engagementConcernRaisedDate;
        }
        
        public EngagementConcernId Id { get; }
        public SupportProjectId SupportProjectId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool? EngagementConcernRecorded { get; set; }
        public string? EngagementConcernDetails { get; set; }
        public DateTime? EngagementConcernRaisedDate { get; set; }
        public bool? EngagementConcernResolved { get; set; }
        public string? EngagementConcernResolvedDetails { get; set; }
        public DateTime? EngagementConcernResolvedDate { get; set; }
        public bool? EngagementConcernEscalationConfirmStepsTaken { get; set; }
        public string? EngagementConcernEscalationPrimaryReason { get; set; }
        public string? EngagementConcernEscalationDetails { get; set; }
        public DateTime? EngagementConcernEscalationDateOfDecision { get; set; }
        public string? EngagementConcernEscalationWarningNotice { get; set; }
        
        public void SetEngagementConcernDetails(bool? engagementConcernRecorded, string? engagementConcernDetails, DateTime? engagementConcernRaisedDate)
        {
            EngagementConcernRecorded = engagementConcernRecorded;
            EngagementConcernDetails = engagementConcernDetails;
            EngagementConcernRaisedDate = engagementConcernRaisedDate;
        }

        public void SetEngagementConcernEscalation(bool? confirmStepsTaken, string? primaryReason,
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