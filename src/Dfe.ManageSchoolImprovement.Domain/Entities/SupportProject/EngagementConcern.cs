using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class EngagementConcern : IEntity<EngagementConcernId>
    {
        public EngagementConcern(EngagementConcernId id, SupportProjectId supportProjectId,
            string? engagementConcernDetails, DateTime? engagementConcernRaisedDate, bool? engagementConcernResolved,
            string? engagementConcernResolvedDetails, DateTime? engagementConcernResolvedDate)
        {
            Id = id;
            SupportProjectId = supportProjectId;
            EngagementConcernDetails = engagementConcernDetails;
            EngagementConcernRaisedDate = engagementConcernRaisedDate;
            EngagementConcernResolved = engagementConcernResolved;
            EngagementConcernResolvedDetails = engagementConcernResolvedDetails;
            EngagementConcernResolvedDate = engagementConcernResolvedDate;
        }

        public EngagementConcernId Id { get; }
        public int ReadableId { get; }
        public SupportProjectId SupportProjectId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
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

        public bool? InformationPowersInUse { get; private set; }
        public string? InformationPowersDetails { get; private set; }
        public DateTime? PowersUsedDate { get; private set; }

        public bool? InterimExecutiveBoardCreated { get; private set; }
        public string? InterimExecutiveBoardCreatedDetails { get; private set; }
        public DateTime? InterimExecutiveBoardCreatedDate { get; private set; }

        public void SetEngagementConcernDetails(string? engagementConcernDetails, DateTime? engagementConcernRaisedDate)
        {
            EngagementConcernDetails = engagementConcernDetails;
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
        public void SetInformationPowersDetails(bool? informationPowersInUse, string? informationPowersDetails,
            DateTime? powersUsedDate)
        {
            InformationPowersInUse = informationPowersInUse;
            InformationPowersDetails = informationPowersDetails;
            PowersUsedDate = powersUsedDate;
        }

        public void SetInterimExecutiveBoardCreated(bool? interimExecutiveBoardCreated,
            string? interimExecutiveBoardCreatedDetails, DateTime? interimExecutiveBoardCreatedDate)
        {
            InterimExecutiveBoardCreated = interimExecutiveBoardCreated;
            InterimExecutiveBoardCreatedDetails = interimExecutiveBoardCreatedDetails;
            InterimExecutiveBoardCreatedDate = interimExecutiveBoardCreatedDate;
        }
    }
}