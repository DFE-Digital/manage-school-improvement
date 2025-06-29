using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Domain.Enums;

public enum FundingBand
{
    [Display(Name = "Reserves exceed funding level", ShortName = "reserves-exceed")]
    ReservesExceedFundingLevel,

    [Display(Name = "Up to \u00A340,000", ShortName = "40000")]
    UpTo40000,

    [Display(Name = "Up to \u00A380,000", ShortName = "80000")]
    UpTo80000,

    [Display(Name = "Up to \u00A3120,000", ShortName = "120000")]
    UpTo120000
}
