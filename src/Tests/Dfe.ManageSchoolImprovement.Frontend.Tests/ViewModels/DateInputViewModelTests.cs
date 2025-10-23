using Dfe.ManageSchoolImprovement.Frontend.ViewModels; 

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.ViewModels
{
    public class DateInputViewModelTests
    {
        [Fact]
        public void DateInputViewModel_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var model = new DateInputViewModel();

            // Assert
            Assert.Null(model.Id);
            Assert.Null(model.Name);
            Assert.Null(model.Day);
            Assert.Null(model.Month);
            Assert.Null(model.Year);
            Assert.Null(model.Label);
            Assert.Null(model.SubLabel);
            Assert.False(model.HeadingLabel);
            Assert.Null(model.Hint);
            Assert.Null(model.ErrorMessage);
            Assert.False(model.DayInvalid);
            Assert.False(model.MonthInvalid);
            Assert.False(model.YearInvalid);
            Assert.False(model.DateMissingOrIncorrect);
            Assert.Null(model.PreviousInformation);
            Assert.Null(model.AdditionalInformation);
            Assert.Null(model.DateString);
            Assert.Null(model.DetailsHeading);
            Assert.Null(model.DetailsBody);
        }

        [Fact]
        public void DateInputViewModel_ShouldSetAndGetValues()
        {
            // Arrange
            var model = new DateInputViewModel
            {
                Id = "date1",
                Name = "birthdate",
                Day = "25",
                Month = "12",
                Year = "1990",
                Label = "Date of Birth",
                SubLabel = "Please enter your birth date",
                HeadingLabel = true,
                Hint = "Enter a valid date",
                ErrorMessage = "Invalid date",
                DayInvalid = false,
                MonthInvalid = false,
                YearInvalid = false,
                DateMissingOrIncorrect = false,
                PreviousInformation = "Previous entry: 01/01/1990",
                AdditionalInformation = "No additional information",
                DateString = "25/12/1990",
                DetailsHeading = "Date Information",
                DetailsBody = "Please enter a valid birthdate."
            };

            // Act & Assert
            Assert.Equal("date1", model.Id);
            Assert.Equal("birthdate", model.Name);
            Assert.Equal("25", model.Day);
            Assert.Equal("12", model.Month);
            Assert.Equal("1990", model.Year);
            Assert.Equal("Date of Birth", model.Label);
            Assert.Equal("Please enter your birth date", model.SubLabel);
            Assert.True(model.HeadingLabel);
            Assert.Equal("Enter a valid date", model.Hint);
            Assert.Equal("Invalid date", model.ErrorMessage);
            Assert.False(model.DayInvalid);
            Assert.False(model.MonthInvalid);
            Assert.False(model.YearInvalid);
            Assert.False(model.DateMissingOrIncorrect);
            Assert.Equal("Previous entry: 01/01/1990", model.PreviousInformation);
            Assert.Equal("No additional information", model.AdditionalInformation);
            Assert.Equal("25/12/1990", model.DateString);
            Assert.Equal("Date Information", model.DetailsHeading);
            Assert.Equal("Please enter a valid birthdate.", model.DetailsBody);
        }
         
        [Fact]
        public void DateInputViewModel_ShouldAllowInvalidDayMonthYearFlags()
        {
            // Arrange
            var model = new DateInputViewModel
            {
                DayInvalid = true,
                MonthInvalid = true,
                YearInvalid = true
            };

            // Act & Assert
            Assert.True(model.DayInvalid);
            Assert.True(model.MonthInvalid);
            Assert.True(model.YearInvalid);
        }
    }
}
