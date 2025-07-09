namespace Dfe.ManageSchoolImprovement.Frontend.ViewModels;

public class SummaryListRowViewModel
{
    public string Id { get; set; } = null!;
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
    public string ValueLink { get; set; } = null!;
    public string AdditionalText { get; set; } = null!;
    public bool HasValue => !string.IsNullOrWhiteSpace(Value);
    public bool HasAdditionalText => !string.IsNullOrWhiteSpace(AdditionalText);
    public bool HasValueLink => !string.IsNullOrWhiteSpace(ValueLink);
    public string Page { get; set; } = null!;
    public string Fragment { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string Return { get; set; } = null!;
    public string HiddenText { get; set; } = null!;
    public string KeyWidth { get; set; } = null!;
    public string ValueWidth { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool HighlightNegativeValue { get; set; }
    public bool IsReadOnly { get; set; }

    public string NegativeStyleClass
    {
        get
        {
            string negativeStyleClass = string.Empty;
            if (HasValue && Decimal.TryParse(Value.Replace("�", ""), out decimal decimalValue))
            {
                    negativeStyleClass = HighlightNegativeValue && decimalValue < 0 ? "negative-value" : string.Empty;
            }

            return negativeStyleClass;
        }
    }
}
