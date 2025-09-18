using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject
{
    public class EngagementConcern : IEntity<EngagementConcernId>
    {
        public EngagementConcern(EngagementConcernId id, SupportProjectId supportProjectId)
        {
            Id = id;
            SupportProjectId = supportProjectId;
        }
        
        public EngagementConcernId Id { get; }
        public SupportProjectId SupportProjectId { get; private set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}