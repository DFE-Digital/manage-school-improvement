using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Shared;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages.Shared;

public class AutosearchUtilsTests
{
    private static SupportProjectDto CreateSupportProject(string schoolName = "Test School", string schoolUrn = "123456") =>
        new(
            Id: 1,
            CreatedOn: DateTime.UtcNow,
            LastModifiedOn: null,
            SchoolName: schoolName,
            SchoolUrn: schoolUrn);

    #region SplitOnBrackets

    [Fact]
    public void SplitOnBrackets_WhenNull_ReturnsSingleEmptyStringElement()
    {
        var result = AutosearchUtils.SplitOnBrackets(null!);

        result.Should().Equal(string.Empty);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void SplitOnBrackets_WhenEmptyOrWhitespace_ReturnsSingleEmptyStringElement(string input)
    {
        var result = AutosearchUtils.SplitOnBrackets(input);

        result.Should().Equal(string.Empty);
    }

    [Fact]
    public void SplitOnBrackets_WhenNoBrackets_ReturnsSingleSegment()
    {
        var result = AutosearchUtils.SplitOnBrackets("Academy Primary");

        result.Should().Equal("Academy Primary");
    }

    [Fact]
    public void SplitOnBrackets_WhenBracketsPresent_SplitsAndTrimsSegments()
    {
        var result = AutosearchUtils.SplitOnBrackets("School Name (123456)");

        result.Should().Equal("School Name", "123456");
    }

    [Fact]
    public void SplitOnBrackets_WhenNestedOrMultipleBracketPairs_ReturnsAllNonEmptySegments()
    {
        var result = AutosearchUtils.SplitOnBrackets("a(b)c");

        result.Should().Equal("a", "b", "c");
    }

    #endregion

    #region HighlightSearchMatch — EstablishmentSearchResponse

    [Fact]
    public void HighlightSearchMatch_School_WhenSchoolIsNull_ReturnsEmpty()
    {
        var result = AutosearchUtils.HighlightSearchMatch("Any (input)", "term", (EstablishmentSearchResponse?)null!);

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void HighlightSearchMatch_School_WhenNameIsMissing_ReturnsEmpty(string name)
    {
        var school = new EstablishmentSearchResponse { Name = name, Urn = "1", Ukprn = "" };

        var result = AutosearchUtils.HighlightSearchMatch("Label", "x", school);

        result.Should().BeEmpty();
    }

    [Fact]
    public void HighlightSearchMatch_School_WhenUrnAndUkprnBothEmpty_ReturnsEmpty()
    {
        var school = new EstablishmentSearchResponse { Name = "School", Urn = "", Ukprn = "" };

        var result = AutosearchUtils.HighlightSearchMatch("School (100)", "School", school);

        result.Should().BeEmpty();
    }

    [Fact]
    public void HighlightSearchMatch_School_WhenUrnPresent_AppliesHighlight()
    {
        var school = new EstablishmentSearchResponse { Name = "School", Urn = "100", Ukprn = "" };

        var result = AutosearchUtils.HighlightSearchMatch("Hill School (100)", "school", school);

        result.Should().Be("Hill <strong>School</strong> (100)");
    }

    [Fact]
    public void HighlightSearchMatch_School_WhenOnlyUkprnPresent_AppliesHighlight()
    {
        var school = new EstablishmentSearchResponse { Name = "School", Urn = "", Ukprn = "999" };

        var result = AutosearchUtils.HighlightSearchMatch("Trust (999)", "999", school);

        result.Should().Be("Trust (<strong>999</strong>)");
    }

    #endregion

    #region HighlightSearchMatch — SupportProjectDto

    [Fact]
    public void HighlightSearchMatch_Project_WhenProjectIsNull_ReturnsEmpty()
    {
        var result = AutosearchUtils.HighlightSearchMatch("Any", "x", (SupportProjectDto?)null!);

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void HighlightSearchMatch_Project_WhenSchoolNameMissing_ReturnsEmpty(string schoolName)
    {
        var project = CreateSupportProject(schoolName: schoolName, schoolUrn: "1");

        var result = AutosearchUtils.HighlightSearchMatch("x", "x", project);

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void HighlightSearchMatch_Project_WhenSchoolUrnMissing_ReturnsEmpty(string urn)
    {
        var project = CreateSupportProject(schoolName: "School", schoolUrn: urn);

        var result = AutosearchUtils.HighlightSearchMatch("School (1)", "School", project);

        result.Should().BeEmpty();
    }

    [Fact]
    public void HighlightSearchMatch_Project_WhenValid_AppliesHighlight()
    {
        var project = CreateSupportProject(schoolName: "Academy", schoolUrn: "42");

        var result = AutosearchUtils.HighlightSearchMatch("North Academy (42)", "academy", project);

        result.Should().Be("North <strong>Academy</strong> (42)");
    }

    #endregion

    #region HighlightSearchMatch — TrustSearchResponse

    [Fact]
    public void HighlightSearchMatch_Trust_WhenTrustIsNull_ReturnsEmpty()
    {
        var result = AutosearchUtils.HighlightSearchMatch("Any", "x", (TrustSearchResponse?)null!);

        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("", "Trust Name")]
    [InlineData("12345678", "")]
    [InlineData("   ", "Name")]
    [InlineData("12345678", "   ")]
    public void HighlightSearchMatch_Trust_WhenUkprnOrNameMissing_ReturnsEmpty(string ukprn, string name)
    {
        var trust = new TrustSearchResponse { Ukprn = ukprn, Name = name };

        var result = AutosearchUtils.HighlightSearchMatch("x", "x", trust);

        result.Should().BeEmpty();
    }

    [Fact]
    public void HighlightSearchMatch_Trust_WhenValid_AppliesHighlight()
    {
        var trust = new TrustSearchResponse { Ukprn = "10001234", Name = "Example Trust" };

        var result = AutosearchUtils.HighlightSearchMatch("Example Trust (10001234)", "Example", trust);

        result.Should().Be("<strong>Example</strong> Trust (10001234)");
    }

    #endregion

    #region HighlightSearchMatch — NameAndCodeDto

    [Fact]
    public void HighlightSearchMatch_LocalAuthority_WhenNull_ReturnsEmpty()
    {
        var result = AutosearchUtils.HighlightSearchMatch("Any", "x", (NameAndCodeDto?)null!);

        result.Should().BeEmpty();
    }

    [Fact]
    public void HighlightSearchMatch_LocalAuthority_WhenCodeMissing_ReturnsEmpty()
    {
        var la = new NameAndCodeDto { Name = "Birmingham", Code = "" };

        var result = AutosearchUtils.HighlightSearchMatch("x", "x", la);

        result.Should().BeEmpty();
    }

    [Fact]
    public void HighlightSearchMatch_LocalAuthority_WhenNameMissing_ReturnsEmpty()
    {
        var la = new NameAndCodeDto { Name = "", Code = "330" };

        var result = AutosearchUtils.HighlightSearchMatch("x", "x", la);

        result.Should().BeEmpty();
    }

    [Fact]
    public void HighlightSearchMatch_LocalAuthority_WhenValid_AppliesHighlight()
    {
        var la = new NameAndCodeDto { Name = "Birmingham City Council", Code = "330" };

        var result = AutosearchUtils.HighlightSearchMatch("Birmingham City Council (330)", "birmingham", la);

        result.Should().Be("<strong>Birmingham</strong> City Council (330)");
    }

    #endregion

    #region HighlightMatchedSubstring (via highlights)

    [Fact]
    public void HighlightSearchMatch_WhenToReplaceIsNullOrWhitespace_ReturnsInputUnchanged()
    {
        var school = new EstablishmentSearchResponse { Name = "S", Urn = "1", Ukprn = "" };
        const string input = "Plain Label (1)";

        AutosearchUtils.HighlightSearchMatch(input, null!, school).Should().Be(input);
        AutosearchUtils.HighlightSearchMatch(input, "", school).Should().Be(input);
        AutosearchUtils.HighlightSearchMatch(input, "   ", school).Should().Be(input);
    }

    [Fact]
    public void HighlightSearchMatch_WhenNoMatch_ReturnsInputUnchanged()
    {
        var school = new EstablishmentSearchResponse { Name = "S", Urn = "1", Ukprn = "" };

        var result = AutosearchUtils.HighlightSearchMatch("Other text", "missing", school);

        result.Should().Be("Other text");
    }

    [Fact]
    public void HighlightSearchMatch_ReplacesAllCaseInsensitiveOccurrences()
    {
        var school = new EstablishmentSearchResponse { Name = "S", Urn = "1", Ukprn = "" };

        var result = AutosearchUtils.HighlightSearchMatch("park park", "park", school);

        result.Should().Be("<strong>park</strong> <strong>park</strong>");
    }

    #endregion
}
