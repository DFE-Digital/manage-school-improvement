using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.ViewModels
{
    public class CheckboxInputViewModelTests
    {
        [Fact]
        public void CheckboxInputViewModel_InitializesWithDefaultValues()
        {
            // Arrange & Act
            var model = new CheckboxInputViewModel();

            // Assert
            Assert.Null(model.Id);
            Assert.Null(model.Name);
            Assert.Null(model.Value);
            Assert.Null(model.Label);
            Assert.Null(model.LabelHint);
            Assert.Null(model.Heading);
            Assert.Null(model.HeadingStyle);
            Assert.Null(model.ErrorMessage);
        }

        [Fact]
        public void CheckboxInputViewModel_SetsValuesCorrectly()
        {
            // Arrange
            var expectedId = "checkbox-123";
            var expectedName = "agreeToTerms";
            var expectedValue = "true";
            var expectedLabel = "I agree to the terms and conditions.";
            var expectedHeadingStyle = "font-weight: bold;";
            var expectedHeading = "Terms and Conditions";
            var expectedLabelHint = "Please read the terms and conditions before agreeing.";
            var expectedErrorMessage = "You must agree to the terms.";

            // Action
            var model = new CheckboxInputViewModel
            {
                Id = expectedId,
                Name = expectedName,
                Value = expectedValue,
                ErrorMessage = expectedErrorMessage,
                Heading = expectedHeading,
                HeadingStyle = expectedHeadingStyle,
                Label = expectedLabel,
                LabelHint = expectedLabelHint,
            };

            // Assert
            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedValue, model.Value);
            Assert.Equal(expectedLabel, model.Label);
            Assert.Equal(expectedHeadingStyle, model.HeadingStyle);
            Assert.Equal(expectedHeading, model.Heading);
            Assert.Equal(expectedLabelHint, model.LabelHint);
            Assert.Equal(expectedErrorMessage, model.ErrorMessage);
        } 
    }

}
