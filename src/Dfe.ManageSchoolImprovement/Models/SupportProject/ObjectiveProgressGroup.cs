namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject
{
    public class ObjectiveProgressGroup
    {
        public string AreaOfImprovement { get; set; } = string.Empty;
        public List<ObjectiveProgressViewModel> ObjectiveProgresses { get; set; } = [];
    }

    public class ObjectiveProgressViewModel
    {
        public Guid Id { get; set; }
        public int ReadableId { get; set; }
        public int ObjectiveReadableId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Progress { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
