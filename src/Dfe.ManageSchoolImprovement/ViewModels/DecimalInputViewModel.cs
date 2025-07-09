namespace Dfe.ManageSchoolImprovement.Frontend.ViewModels;

public class DecimalInputViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Hint { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Suffix { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsMonetary { get; set; }
    public bool HeadingLabel { get; set; }
}
