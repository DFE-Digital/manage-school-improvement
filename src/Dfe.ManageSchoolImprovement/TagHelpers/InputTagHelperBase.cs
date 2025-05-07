using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;

public abstract class InputTagHelperBase(IHtmlHelper htmlHelper) : TagHelper
{
    protected readonly IHtmlHelper _htmlHelper = htmlHelper;

    [HtmlAttributeName("id")]
    public string Id { get; set; }

    [HtmlAttributeName("name")]
    public string Name { get; set; }

    [HtmlAttributeName("label")]
    public string Label { get; set; }

    [HtmlAttributeName("label-hint")]
    public string LabelHint { get; set; }

    [HtmlAttributeName("sub-label")]
    public string SubLabel { get; set; }

    [HtmlAttributeName("previous-information")]
    public string PreviousInformation { get; set; }

    [HtmlAttributeName("additional-information")]
    public string AdditionalInformation { get; set; }

    [HtmlAttributeName("suffix")]
    public string Suffix { get; set; }

    [HtmlAttributeName("asp-for")]
    public ModelExpression For { get; set; }

    [HtmlAttributeName("hint")]
    public string Hint { get; set; }

    [HtmlAttributeName("details-heading")]
    public string DetailsHeading { get; set; }
    [HtmlAttributeName("details-body")]
    public string DetailsBody { get; set; }

    [HtmlAttributeName("heading")]
    public string Heading { get; set; }

    [HtmlAttributeName("heading-style")]
    public string HeadingStyle { get; set; }
    [HtmlAttributeName("error-message")]
    public string ErrorMessage { get; set; }
    
    [HtmlAttributeName("email")]
    public string Email { get; set; }    

    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (_htmlHelper is IViewContextAware viewContextAware)
        {
            viewContextAware.Contextualize(ViewContext);
        }

        if (string.IsNullOrWhiteSpace(Id))
        {
            Id = Name;
        }

        if (string.IsNullOrWhiteSpace(Name))
        {
            Name = Id;
        }

        IHtmlContent content = await RenderContentAsync();
        output.TagName = null;
        output.PostContent.AppendHtml(content);
    }

    protected abstract Task<IHtmlContent> RenderContentAsync();
}
