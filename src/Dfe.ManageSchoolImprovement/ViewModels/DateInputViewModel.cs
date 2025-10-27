namespace Dfe.ManageSchoolImprovement.Frontend.ViewModels;

public class DateInputViewModel
{
    public string Id { get; set; } = null!;
   public string Name { get; set; } = null!;
   public string Day { get; set; } = null!;
   public string Month { get; set; } = null!;
   public string Year { get; set; } = null!;
   public string Label { get; set; } = null!;
   public string SubLabel { get; set; } = null!;
   public bool HeadingLabel { get; set; }
   public string Hint { get; set; } = null!;
   public string ErrorMessage { get; set; } = null!;
   public bool DayInvalid { get; set; }
   public bool MonthInvalid { get; set; }
   public bool YearInvalid { get; set; }
   public bool DateMissingOrIncorrect { get; set; }
   public string PreviousInformation { get; set; } = null!;
   public string AdditionalInformation { get; set; } = null!;
   public string DateString { get; set; } = null!;
   public string DetailsHeading { get; set; } = null!;
   public string DetailsBody { get; set; } = null!;
}
