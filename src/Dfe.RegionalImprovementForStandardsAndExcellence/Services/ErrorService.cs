﻿using Dfe.Academisation.ExtensionMethods;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Services;

public class ErrorService
{
   private const string API_ERROR =
      "There is a system problem and we could not save your changes. Contact regionalservices.rg@education.gov.uk if this continues.";

   private readonly List<Error> _errors = [];
   private static readonly string[] dayUnit = ["-day", "-month", "-year"];

   public void AddError(string key, string message)
   {
      _errors.Add(new Error { Key = key, Message = message });
   }

   public void AddErrors(IEnumerable<string> keys, ModelStateDictionary modelState)
   {
      foreach (string key in keys)
      {
         if (IsDateInputId(key))
         {
            AddDateError(key, modelState);
         }
         else if (modelState.TryGetValue(key, out ModelStateEntry entry) && entry.Errors.Count > 0)
         {
            AddError(key, entry.Errors.Last().ErrorMessage);
         }
      }
   }

   public void AddApiError()
   {
      _errors.Add(new Error { Message = API_ERROR });
   }

   public Error GetError(string key)
   {
      return _errors.SingleOrDefault(e => e.Key == key);
   }

   public IEnumerable<Error> GetErrors()
   {
      return _errors;
   }

   public bool HasErrors()
   {
      return _errors.Count > 0;
   }

   private void AddDateError(string key, ModelStateDictionary modelState)
   {
      if (modelState.TryGetValue(DateInputId(key), out ModelStateEntry dateEntry) && dateEntry.Errors.Count > 0)
      {
         Error dateError = GetError(DateInputId(key));
         if (dateError == null)
         {
            dateError = new Error { Key = DateInputId(key), Message = dateEntry.Errors.First().ErrorMessage.ToSentenceCase() };
            _errors.Add(dateError);
         }

         if (modelState.TryGetValue(key, out ModelStateEntry _))
         {
            dateError.InvalidInputs.Add(key);
         }
      }
   }

   private static bool IsDateInputId(string id)
   {
      return id.EndsWith("-day") || id.EndsWith("-month") || id.EndsWith("-year");
   }

   private static string DateInputId(string id)
   {
      string timeUnit = dayUnit.FirstOrDefault(id.EndsWith);

      return timeUnit is null
         ? id
         : id[..id.LastIndexOf(timeUnit, StringComparison.Ordinal)];
   }
}
