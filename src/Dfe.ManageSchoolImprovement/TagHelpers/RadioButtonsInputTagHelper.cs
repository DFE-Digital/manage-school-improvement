using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;

[HtmlTargetElement("govuk-radiobuttons-input", TagStructure = TagStructure.WithoutEndTag)]
public class RadioButtonsInputTagHelper(IHtmlHelper htmlHelper) : InputTagHelperBase(htmlHelper)
{
    public IList<RadioButtonsLabelViewModel> RadioButtons { get; set; } = [];

    public bool HasError { get; set; } = false;
    public new string? ErrorMessage { get; set; } = null;
    protected override async Task<IHtmlContent> RenderContentAsync()
    {
        RadioButtonViewModel model = new() { Name = Name, Heading = Heading, Value = For.Model?.ToString(), RadioButtons = RadioButtons, Hint = Hint, HeadingStyle = HeadingStyle, ErrorMessage = ErrorMessage };

        return await _htmlHelper.PartialAsync("_RadioButtons", model);
    }
}
