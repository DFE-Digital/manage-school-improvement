using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class ImprovementPlan : IEntity<ImprovementPlanId>
    {
        public ImprovementPlan(ImprovementPlanId id, SupportProjectId supportProjectId)
        {
            Id = id;
            SupportProjectId = supportProjectId;
        }

        public ImprovementPlanId? Id { get; private set; }
        public int ReadableId { get; }
        public SupportProjectId SupportProjectId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool? Complete { get; set; }

        public IEnumerable<ImprovementPlanObjective> ImprovementPlanObjectives => _improvementPlanObjectives.AsReadOnly();
        private readonly List<ImprovementPlanObjective> _improvementPlanObjectives = new();
    }
}
