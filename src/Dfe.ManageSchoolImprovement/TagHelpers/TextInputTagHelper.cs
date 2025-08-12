using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;

[HtmlTargetElement("govuk-text-input", TagStructure = TagStructure.WithoutEndTag)]
public class TextInputTagHelper : InputTagHelperBase
{
    public TextInputTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper) { }

    [HtmlAttributeName("width")]
    public int Width { get; set; }

    public bool HeadingLabel { get; set; }

    protected override async Task<IHtmlContent> RenderContentAsync()
    {
        TextInputViewModel model = new()
        {
            Id = Id,
            Name = Name,
            Label = Label,
            Value = For.Model?.ToString()!,
            Width = Width,
            Hint = Hint,
            HeadingLabel = HeadingLabel
        };

        if (ViewContext.ModelState.TryGetValue(Name, out var entry) && entry.Errors.Count > 0)
        {
            model.ErrorMessage = entry.Errors[0].ErrorMessage;
        }

        return await _htmlHelper.PartialAsync("_TextInput", model);
    }
}
