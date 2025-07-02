using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Dfe.ManageSchoolImprovement.Frontend.Models;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public class ErrorServiceTests
    {
        private readonly ErrorService _errorService = new();

        [Fact]
        public void GivenAddError_CanRetrieveError()
        {
            _errorService.AddError("error_key", "error_message");
            Assert.Equal("error_message", _errorService.GetError("error_key").Message);
        }

        [Fact]
        public void GivenAddTramsError_CanRetrieveTramsError()
        {
            _errorService.AddApiError();
            var errors = _errorService.GetErrors().ToList();
            Assert.Single(errors);
            Assert.Contains("There is a system problem", errors[0].Message);
        }

        [Fact]
        public void GivenAddDateError_CanRetrieveDateError()
        {
            ModelStateDictionary model = new();

            model.AddModelError("deadline", "deadline date should be present");
            model.AddModelError("deadline-day", "deadline date should be present");
            // a date error is defined as any error with key ending in "-day" or "-month" or "-year"
            _errorService.AddErrors(["deadline-day"], model);

            var errors = _errorService.GetErrors().ToList();
            Assert.Single(errors);
            Assert.Equal("deadline", errors[0].Key);
            Assert.Equal("Deadline date should be present", errors[0].Message); 
        }

        [Fact]
        public void GivenMultipleErrorsOnSameDateInputField_RetrievesJustOneError()
        {
            ModelStateDictionary model = new();

            model.AddModelError("deadline", "deadline date should be present");
            _errorService.AddErrors(["deadline-day", "deadline-month"], model);

            var errors = _errorService.GetErrors().ToList();
            Assert.Single(errors);
            Assert.Equal("deadline", errors[0].Key);
            Assert.Equal("Deadline date should be present", errors[0].Message);
        }

        [Fact]
        public void GivenDateErrorsWithMultipleModelStateErrorsForOneField_RetrievesOneErrorWithListOfInvalidInputs()
        {
            ModelStateDictionary model = new();

            model.AddModelError("deadline", "deadline date should be present");
            model.AddModelError("deadline-day", "deadline day should be present");
            model.AddModelError("deadline-month", "deadline month should be present");
            _errorService.AddErrors(["deadline-day", "deadline-month"], model);

            var errors = _errorService.GetErrors().ToList();
            Assert.Single(errors);
            var invalidInputs = errors[0].InvalidInputs;
            Assert.Equal(2, invalidInputs.Count);
            Assert.Contains("deadline-day", invalidInputs);
            Assert.Contains("deadline-month", invalidInputs);
        }

        [Fact]
        public void GivenDateErrorsForMultipleFields_RetrievesOneErrorForEachField()
        {
            ModelStateDictionary model = new();

            model.AddModelError("deadline", "deadline date should be present");
            _errorService.AddErrors(["deadline-day", "deadline-month"], model);
            model.AddModelError("start-date", "start date should be present");
            _errorService.AddErrors(["start-date-month", "start-date-year"], model);

            var errors = _errorService.GetErrors().ToList();
            Assert.Equal(2, errors.Count);

            var deadlineError = errors.Where(x => x.Key == "deadline").ToList();
            Assert.Single(deadlineError);
            Assert.Equal("Deadline date should be present", deadlineError[0].Message); 

            var startDateError = errors.Where(x => x.Key == "start-date").ToList();
            Assert.Single(startDateError);
            Assert.Equal("Start date should be present", startDateError[0].Message);
        }

        [Fact]
        public void GivenADateErrorWithValidModelState_DoesNotRetrieveError()
        {
            ModelStateDictionary model = new();

            _errorService.AddErrors(["deadline-day"], model);

            IEnumerable<Error> errors = _errorService.GetErrors();
            Assert.Empty(errors);
        }

        [Fact]
        public void GivenKeysWithCorrespondingModelStateInvalid_RetrievesErrors()
        {
            ModelStateDictionary model = new();
            model.AddModelError("error_field1", "error in field 1");
            model.AddModelError("error_field2", "error in field 2");

            _errorService.AddErrors(["error_field1", "error_field2", "no_error_field"], model);

            var errors = _errorService.GetErrors().ToList();
            Assert.Equal(2, errors.Count); 
            Assert.Equal("error in field 1", errors.First(x => x.Key == "error_field1").Message);
            Assert.Equal("error in field 2", errors.First(x => x.Key == "error_field2").Message);
            Assert.Equal(0, errors.Count(x => x.Key == "no_error_field"));
        }
    }
}
