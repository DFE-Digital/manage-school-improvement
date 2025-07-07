using Dfe.ManageSchoolImprovement.Frontend.ViewModels; 

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.ViewModels
{
    public class DecimalInputViewModelTests
    {
        [Fact]
        public void DecimalInputViewModel_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var model = new DecimalInputViewModel();

            // Assert
            Assert.Null(model.Id);
            Assert.Null(model.Name);
            Assert.Null(model.Value);
            Assert.Null(model.Hint);
            Assert.Null(model.Label);
            Assert.Null(model.Suffix);
            Assert.Null(model.ErrorMessage);
            Assert.False(model.IsMonetary);
            Assert.False(model.HeadingLabel);
        }

        [Fact]
        public void DecimalInputViewModel_ShouldSetAndGetValues()
        {
            // Arrange
            var model = new DecimalInputViewModel()
            {
                Id = "decimal1",
                Name = "decimal",
                Value = "3.5",
                Label = "Number of things",
                Suffix = "Please enter the right number",
                HeadingLabel = true,
                Hint = "Enter a valid number",
                ErrorMessage = "Invalid number",
                IsMonetary = false,
            };

            // Act & Assert
            Assert.Equal("decimal1", model.Id);
            Assert.Equal("decimal", model.Name);
            Assert.Equal("3.5", model.Value);
            Assert.Equal("Number of things", model.Label);
            Assert.Equal("Please enter the right number", model.Suffix);
            Assert.True(model.HeadingLabel);
            Assert.Equal("Enter a valid number", model.Hint);
            Assert.Equal("Invalid number", model.ErrorMessage);
            Assert.False(model.IsMonetary);
        }
    }
}
