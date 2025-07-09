using Dfe.ManageSchoolImprovement.Frontend.Models;
using System.Reflection;
using static Dfe.ManageSchoolImprovement.Frontend.Models.Links;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class LinksTests
    {
        public LinksTests()
        {
            var fieldInfo = typeof(Links).GetField("_links", BindingFlags.NonPublic | BindingFlags.Static);
            var links = (List<LinkItem>)fieldInfo?.GetValue(null)!;
            links.Clear();
        }

        [Fact]
        public void AddLinkItem_ShouldAddNewLinkItem()
        {
            // Act
            var link = AddLinkItem("/newpage", "Back");

            // Assert
            Assert.IsType<LinkItem>(link);
            Assert.Equal("/newpage", link.Page);
            Assert.Equal("Back", link.BackText);
        }

        [Fact]
        public void AddLinkItem_WithoutBackText_ShouldAddNewLinkItem()
        {
            // Act
            var link = AddLinkItem("/newpage");

            // Assert
            Assert.IsType<LinkItem>(link);
            Assert.Equal("/newpage", link.Page);
            Assert.Equal("Back", link.BackText);
        }

        [Fact]
        public void ByPage_ShouldReturnCorrectLinkItem()
        {
            // Arrange
            AddLinkItem("/testpage", "Back");

            // Act
            var link = ByPage("/testpage");

            // Assert
            Assert.NotNull(link);
            Assert.Equal("/testpage", link.Page);
            Assert.Equal("Back", link.BackText);
        }

        [Fact]
        public void ByPage_WithoutBackText_ShouldReturnCorrectLinkItem()
        {
            // Arrange
            AddLinkItem("/testpage");

            // Act
            var link = ByPage("/testpage");

            // Assert
            Assert.NotNull(link);
            Assert.Equal("/testpage", link.Page);
            Assert.Equal("Back", link.BackText);
        }

        [Fact]
        public void ByPage_ShouldReturnNullIfPageDoesNotExist()
        {
            // Arrange
            AddLinkItem("/existingpage", "Back");

            // Act
            var link = ByPage("/nonexistentpage");

            // Assert
            Assert.Null(link);
        }

        [Fact]
        public void InializeProjectDocumentsEnabled_ShouldSetIsApplicationDocumentsEnabledFlag()
        {
            // Act
            InializeProjectDocumentsEnabled(true);

            // Assert
            Assert.True(IsApplicationDocumentsEnabled);

            // Act
            InializeProjectDocumentsEnabled(false);

            // Assert
            Assert.False(IsApplicationDocumentsEnabled);
        }

        [Fact]
        public void AddLinkItem_ShouldAddLinkItemsForPublicLinks()
        {
            // Act
            var accessibilityLink = Public.Accessibility;
            var cookiePreferencesLink = Public.CookiePreferences;
            var privacyLink = Public.Privacy;
            var cookiePreferencesURLLink = Public.CookiePreferencesURL;

            // Assert
            Assert.Equal("/Public/AccessibilityStatement", accessibilityLink.Page);
            Assert.Equal("/Public/CookiePreferences", cookiePreferencesLink.Page);
            Assert.Equal("/Public/Privacy", privacyLink.Page);
            Assert.Equal("/public/cookie-Preferences", cookiePreferencesURLLink.Page);
        }

        [Theory]
        [InlineData("/TaskList/Index")]
        [InlineData("/TaskList/ContactTheResponsibleBody/index")]
        [InlineData("/TaskList/RecordTheResponsibleBodyResponse/Index")]
        [InlineData("/TaskList/AdviserConflictOfInterest/AdviserConflictOfInterest")]
        [InlineData("/TaskList/AllocateAdviser/AllocateAdviser")]
        [InlineData("/TaskList/SendIntroductoryEmail/Index")]
        [InlineData("/TaskList/ArrangeAdvisersFirstFaceToFaceVisit/ArrangeAdvisersFirstFaceToFaceVisit")]
        [InlineData("/TaskList/CompleteAndSaveInitialDiagnosisTemplate/Index")]
        [InlineData("/TaskList/NoteOfVisit/NoteOfVisit")]
        [InlineData("/TaskList/RecordVisitDateToVisitSchool/Index")]
        [InlineData("/TaskList/ChoosePreferredSupportingOrganisation/Index")]
        [InlineData("/TaskList/RecordMatchingDecision/Index")]
        [InlineData("/TaskList/AddSupportingOrganisationContactDetails/Index")]
        [InlineData("/TaskList/DueDiligenceOnPreferredSupportingOrganisation/Index")]
        [InlineData("/TaskList/RecordSupportingOrganisationAppointment/Index")]
        [InlineData("/TaskList/ShareTheIndicativeFundingBandAndTheImprovementPlanTemplate/Index")]
        [InlineData("/TaskList/RecordImprovementPlanDecision/Index")]
        [InlineData("/TaskList/SendAgreedImprovementPlanForApproval/Index")]
        [InlineData("/TaskList/RequestPlanningGrantOfferLetter/Index")]
        [InlineData("/TaskList/ReviewTheImprovementPlan/Index")]
        [InlineData("/TaskList/RequestImprovementGrantOfferLetter/Index")]
        [InlineData("/TaskList/ConfirmImprovementGrantOfferLetterSent/Index")]
        public void TaskList_Items_ShouldHaveCorrectValues(string expectedPage)
        {
            // Arrange & Act
            var expectedBackText = "Back";
            var taskListType = typeof(TaskList);
            var properties = taskListType.GetFields();

            LinkItem? foundItem = null;

            foreach (var prop in properties)
            {
                if (prop.GetValue(null) is LinkItem item && item.Page == expectedPage)
                {
                    foundItem = item;
                    break;
                }
            }

            // Assert
            Assert.NotNull(foundItem);
            Assert.Equal(expectedPage, foundItem.Page);
            Assert.Equal(expectedBackText, foundItem.BackText);
        }

        [Fact]
        public void AddSchool_WhichSchoolNeedsHelp_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = AddSchool.SelectSchool;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/AddSchool/SelectSchool", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void AddSchool_Summary_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = AddSchool.Summary;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/AddSchool/Summary", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void SchoolList_Index_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = SchoolList.Index;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/SchoolList/Index", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void AboutTheSchool_Index_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = AboutTheSchool.Index;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/AboutTheSchool/Index", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void OfstedReports_Index_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = OfstedReports.Index;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/OfstedReports/Index", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void Notes_Index_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Notes.Index;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Notes/Index", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void Notes_NewNote_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Notes.NewNote;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Notes/NewNote", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void Notes_EditNote_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Notes.EditNote;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Notes/EditNote", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void ProjectAssignment_Index_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = AssignDeliveryOfficer.Index;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/AssignDeliveryOfficer/Index", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }
        [Fact]
        public void Contacts_Index_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Contacts.Index;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Contacts/Index", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }
        [Fact]
        public void Contacts_AddContact_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Contacts.AddContact;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Contacts/AddContact", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }
        [Fact]
        public void Contacts_AddContactDetail_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Contacts.AddContactDetail;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Contacts/AddContactDetail", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }
        [Fact]
        public void Contacts_EditContactDetail_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Contacts.EditContactDetail;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Contacts/EditContactDetail", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }
        [Fact]
        public void Contacts_EditContact_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = Contacts.EditContact;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/Contacts/EditContact", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }

        [Fact]
        public void EngagementConcern_Index_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = EngagementConcern.Index;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/EngagementConcern/Index", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }
        [Fact]
        public void EngagementConcern_RecordEngagementConcern_ShouldHaveCorrectValues()
        {
            // Arrange & Act
            var linkItem = EngagementConcern.RecordEngagementConcern;

            // Assert
            Assert.NotNull(linkItem);
            Assert.Equal("/EngagementConcern/RecordEngagementConcern", linkItem.Page);
            Assert.Equal("Back", linkItem.BackText);
        }
    }
}
