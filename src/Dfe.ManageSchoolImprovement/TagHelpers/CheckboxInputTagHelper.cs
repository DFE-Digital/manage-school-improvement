using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;

[HtmlTargetElement("govuk-checkbox-input", TagStructure = TagStructure.WithoutEndTag)]
public class CheckboxInputTagHelper(IHtmlHelper htmlHelper) : InputTagHelperBase(htmlHelper)
{
    protected override async Task<IHtmlContent> RenderContentAsync()
   {
      CheckboxInputViewModel model = new() { Heading = Heading, Id = Id, Name = Name, LabelHint = LabelHint, Label = Label, Value = For.Model?.ToString(), HeadingStyle = HeadingStyle };

      return await _htmlHelper.PartialAsync("_CheckboxInput", model);
   }
}
