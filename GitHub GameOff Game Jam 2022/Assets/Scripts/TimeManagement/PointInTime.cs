namespace TimeManagement
{
    using System;

    /// <summary>
    /// Defines explicit points in time, defined by a year, month, and season
    /// </summary>
    /// <author>Ben</author>
    public class PointInTime
    {
        // hardcoded definitions of how time should progress
        private static readonly int FIRST_YEAR = 1;
        private static readonly SeasonType[] SEASON_IN_YEAR_SEQUENCE = 
        {
            SeasonType.SPRING, SeasonType.SUMMER, SeasonType.FALL, SeasonType.WINTER
        };
        private static readonly int FIRST_ROUND_IN_SEASON = 1;
        private static readonly int LAST_ROUND_IN_SEASON = 3;

        PointInTime(int year, SeasonType seasonInYear, int monthInSeason)
        {
            Year = year;
            SeasonInYear = seasonInYear;
            RoundInSeason = monthInSeason;
        }

        public int Year { get; private set; }
        public SeasonType SeasonInYear { get; private set; }
        public int RoundInSeason { get; private set; }

        public static PointInTime GenerateFirstPointInTime()
        {
            return new PointInTime(FIRST_YEAR, SEASON_IN_YEAR_SEQUENCE[0], FIRST_ROUND_IN_SEASON);
        }

        public PointInTime GenerateNext()
        {

            int newYear;
            SeasonType newSeason;
            int newRound;

            if (IsLastRoundOfSeason() && IsLastSeasonOfYear())
            { // if at end of year
                newYear = Year + 1;
                newSeason = GetNextSeason(SeasonInYear);
                newRound = GetNextRound(RoundInSeason);
            }
            else if (IsLastRoundOfSeason())
            { // if at end of season but not end of year
                newYear = Year;
                newSeason = GetNextSeason(SeasonInYear);
                newRound = GetNextRound(RoundInSeason);
            }
            else
            { // if just within a season
                newYear = Year;
                newSeason = SeasonInYear;
                newRound = GetNextRound(RoundInSeason);
            }

            return new PointInTime(newYear, newSeason, newRound);
        }

        public override string ToString() => "Year " + Year + ", " + SeasonInYear.ToString() + ", Round " + RoundInSeason;
        public SeasonType GetNextSeason() => GetNextSeason(SeasonInYear);
        public int GetRoundsRemainingInSeason() => LAST_ROUND_IN_SEASON - RoundInSeason;
        public bool IsStartingPointInTime() => Year == FIRST_YEAR && RoundInSeason == FIRST_ROUND_IN_SEASON && SeasonInYear == SEASON_IN_YEAR_SEQUENCE[0];
        public bool IsFirstRoundOfSeason() => RoundInSeason == FIRST_ROUND_IN_SEASON;
        public bool IsFirstSeasonOfYear() => SeasonInYear == SEASON_IN_YEAR_SEQUENCE[0];

        private static int GetSeasonIndex(SeasonType s) => Array.IndexOf(SEASON_IN_YEAR_SEQUENCE, s);

        private bool IsLastRoundOfSeason() => IsLastRoundOfSeason(RoundInSeason);
        private bool IsLastSeasonOfYear() => IsLastSeasonOfYear(SeasonInYear);

        private static bool IsLastSeasonOfYear(SeasonType s) => SEASON_IN_YEAR_SEQUENCE[SEASON_IN_YEAR_SEQUENCE.Length - 1] == s ? true : false;
        private static bool IsLastRoundOfSeason(int r) => r == LAST_ROUND_IN_SEASON;

        private static SeasonType GetNextSeason(SeasonType s) => IsLastSeasonOfYear(s) ? SEASON_IN_YEAR_SEQUENCE[0] : SEASON_IN_YEAR_SEQUENCE[GetSeasonIndex(s) + 1];
        private static int GetNextRound(int r) => IsLastRoundOfSeason(r) ? FIRST_ROUND_IN_SEASON : r + 1;


    }
}
