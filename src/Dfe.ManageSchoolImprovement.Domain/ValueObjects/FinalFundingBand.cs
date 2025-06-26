using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Domain.Enums;

public enum FinalFundingBand
{
    [Display(Name = "No funding required", ShortName = "no-funding-required")]
    ReservesExceedFundingLevel,

    [Display(Name = "Up to £40,000", ShortName = "40000")]
    UpTo40000,

    [Display(Name = "Up to £80,000", ShortName = "80000")]
    UpTo80000,

    [Display(Name = "Up to £120,000", ShortName = "120000")]
    UpTo120000
}