namespace Dfe.ManageSchoolImprovement.Utils
{
    public static class IntExtensions
    {
        public static string ToOrdinalWord(this int number)
        {
            // Common words for first few ordinals; fallback to numeric suffix for larger numbers
            return number switch
            {
                1 => "First",
                2 => "Second",
                3 => "Third",
                4 => "Fourth",
                5 => "Fifth",
                6 => "Sixth",
                7 => "Seventh",
                8 => "Eighth",
                9 => "Ninth",
                10 => "Tenth",
                11 => "Eleventh",
                12 => "Twelfth",
                _ => ToOrdinal(number)
            };
        }

        private static string ToOrdinal(int number)
        {
            int abs = Math.Abs(number);
            int lastTwo = abs % 100;
            string suffix = (lastTwo is 11 or 12 or 13) ? "th" : (abs % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
            return $"{number}{suffix}";
        }
    }
}
