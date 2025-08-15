using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ImprovementPlanObjectiveViewModel
    {
        public Guid Id { get; set; }
        public Guid ImprovementPlanId { get; set; }
        public int ReadableId { get; set; }
        public string AreaOfImprovement { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public int Order { get; set; }

        public static ImprovementPlanObjectiveViewModel Create(ImprovementPlanObjectiveDto dto)
        {
            return new ImprovementPlanObjectiveViewModel
            {
                Id = dto.id,
                ReadableId = dto.readableId,
                ImprovementPlanId = dto.improvementPlanId,
                Order = dto.order,
                AreaOfImprovement = dto.areaOfImprovement,
                Details = dto.details
            };
        }
    }
}