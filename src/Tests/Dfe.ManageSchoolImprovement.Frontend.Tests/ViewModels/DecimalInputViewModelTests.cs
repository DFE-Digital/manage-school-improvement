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
            Assert.Equal(string.Empty, model.Id);
            Assert.Equal(string.Empty, model.Name);
            Assert.Equal(string.Empty, model.Value);
            Assert.Equal(string.Empty, model.Hint);
            Assert.Equal(string.Empty, model.Label);
            Assert.Equal(string.Empty, model.Suffix);
            Assert.Equal(string.Empty, model.ErrorMessage);
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
