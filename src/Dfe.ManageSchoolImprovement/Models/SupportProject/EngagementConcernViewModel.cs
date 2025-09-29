using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class EngagementConcernViewModel
    {
        public required EngagementConcernId Id { get; set; }

        public int ReadableId { get; set; }
        public int SupportProjectId { get; set; }
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

        public bool? InformationPowersInUse { get; set; }
        public string? InformationPowersDetails { get; set; }
        public DateTime? PowersUsedDate { get; set; }

        public bool? InterimExecutiveBoardCreated { get; set; }
        public string? InterimExecutiveBoardCreatedDetails { get; set; }
        public DateTime? InterimExecutiveBoardCreatedDate { get; set; }

        public static EngagementConcernViewModel Create(EngagementConcernDto engagementConcern)
        {
            return new EngagementConcernViewModel
            {
                Id = new EngagementConcernId(engagementConcern.Id),
                ReadableId = engagementConcern.ReadableId,
                SupportProjectId = engagementConcern.SupportProjectId,
                EngagementConcernDetails = engagementConcern.EngagementConcernDetails,
                EngagementConcernRaisedDate = engagementConcern.EngagementConcernRaisedDate,
                EngagementConcernResolved = engagementConcern.EngagementConcernResolved,
                EngagementConcernResolvedDetails = engagementConcern.EngagementConcernResolvedDetails,
                EngagementConcernResolvedDate = engagementConcern.EngagementConcernResolvedDate,
                EngagementConcernEscalationConfirmStepsTaken =
                    engagementConcern.EngagementConcernEscalationConfirmStepsTaken,
                EngagementConcernEscalationPrimaryReason = engagementConcern.EngagementConcernEscalationPrimaryReason,
                EngagementConcernEscalationDetails = engagementConcern.EngagementConcernEscalationDetails,
                EngagementConcernEscalationDateOfDecision = engagementConcern.EngagementConcernEscalationDateOfDecision,
                EngagementConcernEscalationWarningNotice = engagementConcern.EngagementConcernEscalationWarningNotice,
                InformationPowersInUse = engagementConcern.InformationPowersInUse,
                InformationPowersDetails = engagementConcern.InformationPowersDetails,
                PowersUsedDate = engagementConcern.PowersUsedDate,
                InterimExecutiveBoardCreated = engagementConcern.InterimExecutiveBoardCreated,
                InterimExecutiveBoardCreatedDetails = engagementConcern.InterimExecutiveBoardCreatedDetails,
                InterimExecutiveBoardCreatedDate = engagementConcern.InterimExecutiveBoardCreatedDate
            };
        }
    }
}
