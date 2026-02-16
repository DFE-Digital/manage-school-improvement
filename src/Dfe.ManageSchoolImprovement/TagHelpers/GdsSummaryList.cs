using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;


/// <summary>
/// Usage:
/// <gds-summary-list model="Model" items="new() { m => m.Name, m => m.DateOfBirth }" />
/// </summary>
[HtmlTargetElement("govuk-summary-list", Attributes = "model,items")]
public class GdsSummaryListTagHelper : TagHelper
{
    /// <summary>The model instance being displayed.</summary>
    public object Model { get; set; } = default!;

    /// <summary>List of property selectors to display as rows.</summary>
    public List<LambdaExpression> Items { get; set; } = new();

    /// <summary>Optional: Per-property custom formatters (overrides default formatting).</summary>
    /// <example>Formatters[nameof(User.StartDate)] = o => ((DateTime?)o)?.ToString("dd MMM yyyy") ?? "—";</example>
    public Dictionary<string, Func<object?, string>>? Formatters { get; set; }

    /// <summary>Optional: Text to show when a value is null or empty.</summary>
    public string NullDisplayText { get; set; } = "—";

    /// <summary>Optional: Culture to use for date/number formatting (defaults to current).</summary>
    public string? Culture { get; set; }

    /// <summary>Optional: Render an actions column (e.g., a 'Change' link) per row.</summary>
    public Func<string /*propertyName*/, IHtmlContent?>? ActionContentFactory { get; set; }

    private CultureInfo CultureInfo => string.IsNullOrWhiteSpace(Culture)
        ? CultureInfo.CurrentUICulture
        : new CultureInfo(Culture);

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "dl";
        output.TagMode = TagMode.StartTagAndEndTag; // Ensures <dl>...</dl>
        output.Attributes.SetAttribute("class", "govuk-summary-list");

        var html = new StringBuilder();

        foreach (var expr in Items)
        {
            var (propName, displayName, value) = Evaluate(expr);
            var renderedValue = RenderValue(propName, value);
            var actions = ActionContentFactory?.Invoke(propName);

            html.AppendLine(@"<div class=""govuk-summary-list__row"">");
            html.Append($@"<dt class=""govuk-summary-list__key"">{HtmlEncoder.Default.Encode(displayName)}</dt>");
            html.Append(@"<dd class=""govuk-summary-list__value"">");
            html.Append(renderedValue);
            html.Append("</dd>");

            if (actions != null)
            {
                html.Append(@"<dd class=""govuk-summary-list__actions"">");
                using var writer = new System.IO.StringWriter();
                actions.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                html.Append(writer.ToString());
                html.Append("</dd>");
            }

            html.AppendLine("</div>");
        }

        output.Content.SetHtmlContent(html.ToString());
        await Task.CompletedTask;
    }

    private (string PropName, string DisplayName, object? Value) Evaluate(LambdaExpression exp)
    {
        // Get the member expression from the lambda
        var member = exp.Body as MemberExpression
                     ?? (exp.Body as UnaryExpression)?.Operand as MemberExpression;

        if (member == null)
            throw new InvalidOperationException("Expression must target a property, e.g., m => m.Name.");

        if (member.Member is not PropertyInfo propInfo)
            throw new InvalidOperationException("Expression must target a property, not a field or method.");

        // Display name resolution: [Display(Name)] > [DisplayName] > Property.Name
        var displayAttr = propInfo.GetCustomAttribute<DisplayAttribute>();
        var displayNameAttr = propInfo.GetCustomAttribute<DisplayNameAttribute>();

        var displayName = displayAttr?.GetName()
                           ?? displayNameAttr?.DisplayName
                           ?? propInfo.Name;

        // Compile and evaluate the lambda expression
        var compiled = exp.Compile();
        var value = compiled.DynamicInvoke(Model);

        return (propInfo.Name, displayName!, value);
    }

    private string RenderValue(string propName, object? value)
    {
        // Custom formatter first
        if (Formatters != null && Formatters.TryGetValue(propName, out var formatter))
        {
            var formatted = formatter(value);
            return string.IsNullOrEmpty(formatted)
                ? HtmlEncoder.Default.Encode(NullDisplayText)
                : formatted; // Assume formatter returns HTML-safe content if needed
        }

        // Default formatting
        if (value is null) return HtmlEncoder.Default.Encode(NullDisplayText);

        switch (value)
        {
            case string s:
                return HtmlEncoder.Default.Encode(string.IsNullOrWhiteSpace(s) ? NullDisplayText : s);

            case bool b:
                return HtmlEncoder.Default.Encode(b ? "Yes" : "No");

            case Enum e:
                // Use [Display(Name=...)] if present on enum member
                var enumDisplay = GetEnumDisplayName(e) ?? e.ToString();
                return HtmlEncoder.Default.Encode(enumDisplay);

            case DateOnly dOnly:
                return HtmlEncoder.Default.Encode(dOnly.ToString("dd MMM yyyy", CultureInfo));

            case DateTime dt:
                // Display date only if time is midnight; otherwise include time
                var format = (dt.TimeOfDay == TimeSpan.Zero)
                    ? "dd MMM yyyy"
                    : "dd MMM yyyy HH:mm";
                return HtmlEncoder.Default.Encode(dt.ToString(format, CultureInfo));

            case DateTimeOffset dto:
                var f2 = (dto.TimeOfDay == TimeSpan.Zero)
                    ? "dd MMM yyyy"
                    : "dd MMM yyyy HH:mm";
                return HtmlEncoder.Default.Encode(dto.ToString(f2, CultureInfo));

            case IFormattable formattable:
                // Numbers, decimals, etc.
                return HtmlEncoder.Default.Encode(formattable.ToString(null, CultureInfo));

            default:
                return HtmlEncoder.Default.Encode(value.ToString() ?? NullDisplayText);
        }
    }

    private static string? GetEnumDisplayName(Enum value)
    {
        var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        if (member == null) return null;

        var display = member.GetCustomAttribute<DisplayAttribute>();
        if (display?.GetName() is string dn) return dn;

        var dispName = member.GetCustomAttribute<DisplayNameAttribute>();
        return dispName?.DisplayName;
    }

}

