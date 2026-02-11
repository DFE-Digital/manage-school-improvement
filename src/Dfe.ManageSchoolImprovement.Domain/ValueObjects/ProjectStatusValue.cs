using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public enum ProjectStatusValue
{
    [Display(Name = "In progress")]
    InProgress = 0,
    [Display(Name = "Paused")]
    Paused = 1,
    [Display(Name = "Stopped")]
    Stopped = 2
}