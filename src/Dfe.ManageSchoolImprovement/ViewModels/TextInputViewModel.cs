namespace Dfe.ManageSchoolImprovement.Frontend.ViewModels;

public class TextInputViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string Label { get; set; } = null!;
    public string ErrorMessage { get; set; } = null!;
    public int Width { get; set; }
    public string Hint { get; set; } = null!;
    public bool HeadingLabel { get; set; }
}
