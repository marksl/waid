using System;

namespace Waid
{
    internal static class WatchMe
    {
        public static bool IsNowAGoodTime()
        {
            var userSettings = Properties.Settings.Default;

            DateTime now = DateTime.Now;

            return (TodayIsAGoodDay(userSettings, now.DayOfWeek) && ThisIsAGoodHour(userSettings, now.Hour));
        }

        private static bool ThisIsAGoodHour(Properties.Settings userSettings, int hour)
        {
            return hour >= userSettings.StartTime && hour < userSettings.EndTime;
        }

        private static bool TodayIsAGoodDay(Properties.Settings userSettings, DayOfWeek today)
        {
            bool goodDay = today == DayOfWeek.Sunday && userSettings.Sunday
                   || today == DayOfWeek.Monday && userSettings.Monday
                   || today == DayOfWeek.Tuesday && userSettings.Tues
                   || today == DayOfWeek.Wednesday && userSettings.Weds
                   || today == DayOfWeek.Thursday && userSettings.Thurs
                   || today == DayOfWeek.Friday && userSettings.Fri
                   || today == DayOfWeek.Saturday && userSettings.Sat;

            Logger.DebugFormat("Today is a good day {0} {1}", today, goodDay);
            return goodDay;
        }
    }
}