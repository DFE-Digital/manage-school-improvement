using Dfe.Academisation.ExtensionMethods;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.TagHelpers;

[HtmlTargetElement("govuk-date-input", TagStructure = TagStructure.WithoutEndTag)]

public class DateInputTagHelper(IHtmlHelper htmlHelper, ErrorService errorService) : InputTagHelperBase(htmlHelper)
{
   public bool HeadingLabel { get; set; }

   protected override async Task<IHtmlContent> RenderContentAsync()
   {
      DateInputViewModel model = ValidateRequest();

      return await _htmlHelper.PartialAsync("_DateInput", model);
   }

   private DateInputViewModel ValidateRequest()
   {
      if (For.ModelExplorer.ModelType != typeof(DateTime?))
      {
         throw new ArgumentException("ModelType is not a DateTime?");
      }

      DateTime? date = For.Model as DateTime?;
      DateInputViewModel model = new()
      {
         Id = Id,
         Name = Name,
         Label = Label,
         SubLabel = SubLabel,
         HeadingLabel = HeadingLabel,
         Hint = Hint,
         PreviousInformation = PreviousInformation,
         AdditionalInformation = AdditionalInformation,
         DateString = date.ToDateString(),
         DetailsHeading = DetailsHeading,
         DetailsBody = DetailsBody
      };

      if (date.HasValue)
      {
         model.Day = date.Value.Day.ToString();
         model.Month = date.Value.Month.ToString();
         model.Year = date.Value.Year.ToString();
      }

      Error error = errorService.GetError(Name);
      if (error is not null)
      {
         model.ErrorMessage = error.Message;
         model.DayInvalid = error.Message.Contains("day", StringComparison.OrdinalIgnoreCase);
         if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-day", out StringValues dayValue))
         {
            model.Day = dayValue!;
         }

         model.MonthInvalid = error.Message.Contains("month", StringComparison.OrdinalIgnoreCase);
         if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-month", out StringValues monthValue))
         {
            model.Month = monthValue!;
         }

         model.YearInvalid = error.Message.Contains("year", StringComparison.OrdinalIgnoreCase);
         if (ViewContext.HttpContext.Request.Form.TryGetValue($"{Name}-year", out StringValues yearValue))
         {
            model.Year = yearValue!;
         }

         if (model.ErrorMessage == "Enter a date" || 
             model.ErrorMessage == "You must enter today's date or a date in the past" || 
             model.ErrorMessage == "Enter a date in the correct format")
         {
            model.DateMissingOrIncorrect = true;
         }
         
      }

      return model;
   }
}
