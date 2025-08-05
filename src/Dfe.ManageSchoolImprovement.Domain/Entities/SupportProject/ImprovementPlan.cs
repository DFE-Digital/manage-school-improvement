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
        public bool? ObjectivesSectionComplete { get; set; }

        public IEnumerable<ImprovementPlanObjective> ImprovementPlanObjectives => _improvementPlanObjectives.AsReadOnly();
        private readonly List<ImprovementPlanObjective> _improvementPlanObjectives = new();

        public IEnumerable<ImprovementPlanReview> ImprovementPlanReviews => _improvementPlanReviews.AsReadOnly();
        private readonly List<ImprovementPlanReview> _improvementPlanReviews = new();

        public void AddObjective(ImprovementPlanObjectiveId improvementPlanObjectiveId, ImprovementPlanId improvementPlanId, string areaOfImprovement, string details, int order)
        {
            _improvementPlanObjectives.Add(new ImprovementPlanObjective(improvementPlanObjectiveId, improvementPlanId, areaOfImprovement, details, order));
        }

        public void SetObjectivesComplete(bool objectivesSectionComplete)
        {
            ObjectivesSectionComplete = objectivesSectionComplete;
        }

        public void SetObjectiveDetails(ImprovementPlanObjectiveId improvementPlanObjectiveId, string details)
        {
            var objective = _improvementPlanObjectives.FirstOrDefault(o => o.Id == improvementPlanObjectiveId);

            if (objective == null)
            {
                throw new KeyNotFoundException($"Improvement plan objective with id {improvementPlanObjectiveId} not found");
            }

            objective.SetDetails(details);
        }
    }
}
