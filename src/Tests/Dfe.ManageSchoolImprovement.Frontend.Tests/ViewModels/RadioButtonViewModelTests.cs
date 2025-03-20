
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.ViewModels
{
    public class RadioButtonViewModelTests
    {
        [Fact]
        public void RadioButtonViewModel_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var model = new RadioButtonViewModel();

            // Assert
            Assert.Null(model.Heading);
            Assert.Null(model.HeadingStyle);
            Assert.Null(model.Hint);
            Assert.Null(model.Name);
            Assert.Empty(model.RadioButtons);
            Assert.Null(model.Value);
        }

        [Fact]
        public void RadioButtonViewModel_ShouldSetAndGetValues()
        {
            // Arrange
            var model = new RadioButtonViewModel
            {
                Heading = "Radio Button Group",
                HeadingStyle = "font-weight: bold;",
                Hint = "Please select an option",
                Name = "radioGroup1",
                RadioButtons =
                [
                    new RadioButtonsLabelViewModel
                    {
                        Name = "Option 1",
                        Id = "option1",
                        Value = "value1",
                        Input = new TextFieldInputViewModel
                        {
                            Id = "textarea1",
                            ValidationMessage = "This is required.",
                            Paragraph = "Please provide input for option 1.",
                            Value = "Some text",
                            IsTextArea = true
                        }
                    },
                    new RadioButtonsLabelViewModel
                    {
                        Name = "Option 2",
                        Id = "option2",
                        Value = "value2",
                        Input = new TextFieldInputViewModel
                        {
                            Id = "textarea2",
                            ValidationMessage = "This is required.",
                            Paragraph = "Please provide input for option 2.",
                            Value = "Another text",
                            IsTextArea = true
                        }
                    }
                ],
                Value = "value1"
            };

            // Act & Assert
            Assert.Equal("Radio Button Group", model.Heading);
            Assert.Equal("font-weight: bold;", model.HeadingStyle);
            Assert.Equal("Please select an option", model.Hint);
            Assert.Equal("radioGroup1", model.Name);
            Assert.Equal(2, model.RadioButtons.Count);
            Assert.Equal("value1", model.Value);
        }

        [Fact]
        public void RadioButtonViewModel_ShouldAllowNullValues()
        {
            // Arrange
            var model = new RadioButtonViewModel
            {
                Heading = null,
                HeadingStyle = null,
                Hint = null,
                Name = null,
                RadioButtons = [],
                Value = null
            };

            // Act & Assert
            Assert.Null(model.Heading);
            Assert.Null(model.HeadingStyle);
            Assert.Null(model.Hint);
            Assert.Null(model.Name);
            Assert.Empty(model.RadioButtons);
            Assert.Null(model.Value);
        }
    }

    public class RadioButtonsLabelViewModelTests
    {
        [Fact]
        public void RadioButtonsLabelViewModel_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var model = new RadioButtonsLabelViewModel
            {
                Name = "Test Option",
                Id = "testOption"
            };

            // Assert
            Assert.Equal("Test Option", model.Name);
            Assert.Equal("testOption", model.Id);
            Assert.Null(model.Value);
            Assert.Null(model.Input);
        }

        [Fact]
        public void RadioButtonsLabelViewModel_ShouldSetAndGetValues()
        {
            // Arrange
            var model = new RadioButtonsLabelViewModel
            {
                Name = "Option 1",
                Id = "option1",
                Value = "value1",
                Input = new TextFieldInputViewModel
                {
                    Id = "textarea1",
                    ValidationMessage = "This is required.",
                    Paragraph = "Please provide input for option 1.",
                    Value = "Some text",
                    IsTextArea = true
                }
            };

            // Act & Assert
            Assert.Equal("Option 1", model.Name);
            Assert.Equal("option1", model.Id);
            Assert.Equal("value1", model.Value);
            Assert.NotNull(model.Input);
            Assert.Equal("textarea1", model.Input?.Id);
            Assert.Equal("Some text", model.Input?.Value);
        }
    }

    public class TextAreaInputViewModelTests
    {
        [Fact]
        public void TextAreaInputViewModel_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var model = new TextFieldInputViewModel
            {
                Id = "textarea1",
                ValidationMessage = "This is required.",
                Paragraph = "Please provide input.",
                Value = "Some input",
                IsTextArea = true
            };

            // Assert
            Assert.Equal("textarea1", model.Id);
            Assert.Equal("This is required.", model.ValidationMessage);
            Assert.Equal("Please provide input.", model.Paragraph);
            Assert.Equal("Some input", model.Value);
            Assert.True(model.IsValid);
        }

        [Fact]
        public void TextAreaInputViewModel_ShouldSetAndGetValues()
        {
            // Arrange
            var model = new TextFieldInputViewModel
            {
                Id = "textarea1",
                ValidationMessage = "This is required.",
                Paragraph = "Please provide input.",
                Value = "Test input",
                IsTextArea = true
            };

            // Act & Assert
            Assert.Equal("textarea1", model.Id);
            Assert.Equal("This is required.", model.ValidationMessage);
            Assert.Equal("Please provide input.", model.Paragraph);
            Assert.Equal("Test input", model.Value);
        }

        [Fact]
        public void TextAreaInputViewModel_ShouldAllowNullValue()
        {
            // Arrange
            var model = new TextFieldInputViewModel
            {
                Id = "textarea1",
                ValidationMessage = "This is required.",
                Paragraph = "Please provide input.",
                Value = null,
                IsTextArea = true
            };

            // Act & Assert
            Assert.Null(model.Value);
        }
    }
}
