using Dfe.Academisation.ExtensionMethods;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;

[HtmlTargetElement("govuk-decimal-input", TagStructure = TagStructure.WithoutEndTag)]
public class DecimalInputTagHelper : InputTagHelperBase
{
    private readonly ErrorService _errorService;

    public DecimalInputTagHelper(IHtmlHelper htmlHelper, ErrorService errorService) : base(htmlHelper)
    {
        _errorService = errorService;
    }

    [HtmlAttributeName("isMonetary")]
    public bool IsMonetary { get; set; }

    public bool HeadingLabel { get; set; }

    protected override async Task<IHtmlContent> RenderContentAsync()
    {
        DecimalInputViewModel model = ValidateModel();

        return await _htmlHelper.PartialAsync("_DecimalInput", model);
    }

    private DecimalInputViewModel ValidateModel()
    {
        if (For.ModelExplorer.ModelType != typeof(decimal?) && For.ModelExplorer.ModelType != typeof(decimal))
        {
            throw new ArgumentException("For.ModelExplorer.ModelType is not a decimal");
        }

        decimal? value = (decimal?)For.Model;
        DecimalInputViewModel model = new()
        {
            Id = Id,
            Name = Name,
            Label = Label,
            Hint = Hint,
            Suffix = Suffix,
            Value = IsMonetary ? value?.ToMoneyString() : value.ToSafeString(),
            IsMonetary = IsMonetary,
            HeadingLabel = HeadingLabel
        };

        Error error = _errorService.GetError(Name);
        if (error != null)
        {
            model.ErrorMessage = error.Message;
            if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}", out StringValues invalidValue))
            {
                model.Value = invalidValue;
            }
        }

        return model;
    }
}
