﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.ManageSchoolImprovement.Frontend.ViewModels;

[ExcludeFromCodeCoverage]
public class CheckboxInputModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        Type modelType = bindingContext.ModelType;
        if (modelType != typeof(bool) && modelType != typeof(bool?))
        {
            throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
        }

        string checkboxInputModelName = $"{bindingContext.ModelName}";
        string hiddenInputModelName = $"{bindingContext.ModelName}-hidden";

        ValueProviderResult checkboxInputValueProviderResult = bindingContext.ValueProvider.GetValue(checkboxInputModelName);
        ValueProviderResult hiddenInputValueProviderResult = bindingContext.ValueProvider.GetValue(hiddenInputModelName);

        if (checkboxInputValueProviderResult == ValueProviderResult.None
            && hiddenInputValueProviderResult == ValueProviderResult.None)
        {
            if (modelType == typeof(bool?))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }


        if (checkboxInputValueProviderResult != ValueProviderResult.None && bool.TryParse(checkboxInputValueProviderResult.FirstValue, out bool checkboxInputValue))
        {
            bindingContext.Result = ModelBindingResult.Success(checkboxInputValue);
        }
        else if (hiddenInputValueProviderResult != ValueProviderResult.None && bool.TryParse(hiddenInputValueProviderResult.FirstValue, out bool hiddenInputValue))
        {
            bindingContext.Result = ModelBindingResult.Success(hiddenInputValue);
        }
        else
        {
            bindingContext.ModelState.SetModelValue(checkboxInputModelName, checkboxInputValueProviderResult);
            bindingContext.ModelState.SetModelValue(hiddenInputModelName, hiddenInputValueProviderResult);

            bindingContext.ModelState.TryAddModelError(
               bindingContext.ModelName,
               new ArgumentException("Not a valid boolean checkbox"),
               bindingContext.ModelMetadata);

            bindingContext.Result = ModelBindingResult.Failed();
        }

        return Task.CompletedTask;
    }
}
