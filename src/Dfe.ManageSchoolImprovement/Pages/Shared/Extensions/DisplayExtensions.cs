using Dfe.Academisation.ExtensionMethods;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Linq;

namespace Dfe.ManageSchoolImprovement.Frontend.Extensions;

public static class DisplayExtensions
{
   private const string NoData = "No data";

   public static string DisplayOfstedRating(this string ofstedRating)
   {
      return ofstedRating switch
      {
         "1" => "Outstanding",
         "2" => "Good",
         "3" => "Requires improvement",
         "4" => "Inadequate",
         "5" => "Insufficient evidence",
         "6" => "No judgement",
         "7" => "Does not apply",
         "8" => "Not yet inspected",
         _ => "Unknown"
      };
   }



   public static string GetErrorStyleClass(this ModelStateDictionary modelState)
   {
      return !modelState.IsValid ? "govuk-form-group--error" : "";
   }

   public static string GetTextAreaErrorStyles(this ModelStateDictionary modelState, string field)
   {
      return !modelState.IsValid && modelState.ContainsKey(field) ? "govuk-textarea--error" : string.Empty;
   }

   public static string FormatConfidenceInterval(decimal? lowerBound, decimal? upperBound)
   {
      if (lowerBound == null && upperBound == null) return NoData;
      return $"{lowerBound.FormatValue()} to {upperBound.FormatValue()}";
   }

   public static string FormatValue(this string value)
   {
      return string.IsNullOrEmpty(value) ? NoData : value.FormatAsDouble();
   }

   public static string FormatValue(this decimal? value)
   {
      return value == null ? NoData : value.ToSafeString();
   }

   public static string FormatAsDouble(this string value)
   {
      bool isDouble = double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out double resultAsDouble);
      return isDouble ? $"{resultAsDouble}" : value;
   }

   public static string FormatKeyStageYear(this string year)
   {
      if (string.IsNullOrEmpty(year)) return year;
      string trimmedYear = string.Concat(year.Where(c => !char.IsWhiteSpace(c)));
      return trimmedYear.Contains('-') ? trimmedYear.Replace("-", " to ") : year;
   }

   public static bool HasData(this string str)
   {
      return str != NoData;
   }
}
