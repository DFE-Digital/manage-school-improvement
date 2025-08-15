namespace Dfe.ManageSchoolImprovement.Utils.Tests
{
    public class IntExtensionsTests
    {
        [Theory]
        [InlineData(1, "First")]
        [InlineData(2, "Second")]
        [InlineData(3, "Third")]
        [InlineData(4, "Fourth")]
        [InlineData(5, "Fifth")]
        [InlineData(6, "Sixth")]
        [InlineData(7, "Seventh")]
        [InlineData(8, "Eighth")]
        [InlineData(9, "Ninth")]
        [InlineData(10, "Tenth")]
        [InlineData(11, "Eleventh")]
        [InlineData(12, "Twelfth")]
        public void ToOrdinalWord_Should_Return_CorrectWord_ForNumbersOneToTwelve(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWord());
        }

        [Theory]
        [InlineData(13, "13th")]
        [InlineData(14, "14th")]
        [InlineData(21, "21st")]
        [InlineData(22, "22nd")]
        [InlineData(23, "23rd")]
        [InlineData(24, "24th")]
        [InlineData(31, "31st")]
        [InlineData(32, "32nd")]
        [InlineData(33, "33rd")]
        [InlineData(101, "101st")]
        [InlineData(102, "102nd")]
        [InlineData(103, "103rd")]
        [InlineData(104, "104th")]
        public void ToOrdinalWord_Should_Return_CorrectOrdinal_ForNumbersAboveTwelve(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWord());
        }

        [Theory]
        [InlineData(11, "Eleventh")]
        [InlineData(12, "Twelfth")]
        [InlineData(13, "13th")]
        [InlineData(111, "111th")]
        [InlineData(112, "112th")]
        [InlineData(113, "113th")]
        [InlineData(211, "211th")]
        [InlineData(212, "212th")]
        [InlineData(213, "213th")]
        public void ToOrdinalWord_Should_Return_ThSuffix_ForSpecialCases(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWord());
        }

        [Theory]
        [InlineData(0, "0th")]
        [InlineData(20, "20th")]
        [InlineData(30, "30th")]
        [InlineData(40, "40th")]
        [InlineData(50, "50th")]
        [InlineData(100, "100th")]
        [InlineData(1000, "1000th")]
        public void ToOrdinalWord_Should_Return_ThSuffix_ForMultiplesOfTen(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWord());
        }

        [Theory]
        [InlineData(-1, "-1st")]
        [InlineData(-2, "-2nd")]
        [InlineData(-3, "-3rd")]
        [InlineData(-4, "-4th")]
        [InlineData(-11, "-11th")]
        [InlineData(-21, "-21st")]
        [InlineData(-22, "-22nd")]
        [InlineData(-23, "-23rd")]
        public void ToOrdinalWord_Should_HandleNegativeNumbers_Correctly(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWord());
        }

        [Theory]
        [InlineData(121, "121st")]
        [InlineData(122, "122nd")]
        [InlineData(123, "123rd")]
        [InlineData(124, "124th")]
        [InlineData(1021, "1021st")]
        [InlineData(1022, "1022nd")]
        [InlineData(1023, "1023rd")]
        [InlineData(1024, "1024th")]
        public void ToOrdinalWord_Should_HandleLargeNumbers_Correctly(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWord());
        }

        [Fact]
        public void ToOrdinalWord_Should_HandleZero_Correctly()
        {
            Assert.Equal("0th", 0.ToOrdinalWord());
        }

        [Fact]
        public void ToOrdinalWord_Should_HandleMaxValue_Correctly()
        {
            var result = int.MaxValue.ToOrdinalWord();
            Assert.EndsWith("th", result);
            Assert.StartsWith(int.MaxValue.ToString(), result);
        }

        [Theory]
        [InlineData(91, "91st")]
        [InlineData(92, "92nd")]
        [InlineData(93, "93rd")]
        [InlineData(94, "94th")]
        [InlineData(95, "95th")]
        [InlineData(96, "96th")]
        [InlineData(97, "97th")]
        [InlineData(98, "98th")]
        [InlineData(99, "99th")]
        public void ToOrdinalWord_Should_HandleNinetiesRange_Correctly(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWord());
        }
    }
}