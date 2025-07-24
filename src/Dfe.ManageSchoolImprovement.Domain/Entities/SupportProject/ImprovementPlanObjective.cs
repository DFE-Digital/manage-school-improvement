using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ImprovementPlanObjective : IEntity<ImprovementPlanObjectiveId>
    {
        public ImprovementPlanObjective(ImprovementPlanObjectiveId id, ImprovementPlanId improvementPlanId, string areaOfImprovement, string details, int order)
        {
            Id = id;
            ImprovementPlanId = improvementPlanId;
            AreaOfImprovement = areaOfImprovement;
            Details = details;
            Order = order;
        }

        public ImprovementPlanObjectiveId? Id { get; private set; }
        public int ReadableId { get; }
        public ImprovementPlanId ImprovementPlanId { get; private set; }
        public int Order { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public string AreaOfImprovement { get; set; }
        public string Details { get; set; } = string.Empty;

        public void SetDetails(string details)
        {
            Details = details;
        }
    }
}
