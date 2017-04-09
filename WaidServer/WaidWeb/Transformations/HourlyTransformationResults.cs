using System;
using System.Collections.Generic;
using System.Linq;
using Waid.WindowsAzure;

namespace WaidWeb.Transformations
{
    public class HourlyTransformationResults
    {
        public HourlyTransformationResults(int minutesOffset)
        {
            _minutesOffset = minutesOffset;
            _filteredSeconds = new List<float>();
            _filteredApps = new List<uint>();
            _overflowFilteredSeconds = new List<float>();
            _overflowFilteredApps = new List<uint>();
        }

        private readonly int _minutesOffset;
        private readonly List<float> _filteredSeconds;
        private readonly List<uint> _filteredApps;
        private readonly List<float> _overflowFilteredSeconds;
        private readonly List<uint> _overflowFilteredApps;

        public HourlyUsage ToHourlyUsage()
        {
            var usage = new HourlyUsage
            {
                AppUsedSeconds = _filteredSeconds.ToArray(),
                AppUsedNameHashCodes = _filteredApps.ToArray()
            };
            return usage;
        }
        
        private void AddGapIfTimeGreaterThan2Seconds(DateTime currentTime, float secondsElapsed)
        {
            var secondsGap =
                (float)currentTime.Subtract(_startTime.Value).TotalSeconds - secondsElapsed;

            if (secondsGap > 2.0f)
            {
                Log(noActivityApp, secondsGap);
            }
        }

        static readonly uint noActivityApp = StringUtils.HashString(UsageRepository.noActivity);

        private DateTime? _startTime;
        private DateTime _startTimeUTC;

        private float _secondsElapsed;
        //private DateTime _currentTime;

        public void StartRow(UsageRow row)
        {
            DateTime rowTime = row.GetStartTime(_minutesOffset);

            if (!_startTime.HasValue)
            {
                _startTime = new DateTime(rowTime.Year, rowTime.Month, rowTime.Day, rowTime.Hour, 0, 0);
                _startTimeUTC = row.GetStartTimeUtc();
                _secondsElapsed = 0.0f;
            }

            AddGapIfTimeGreaterThan2Seconds(rowTime, _secondsElapsed);


            TimeSpan ts = rowTime.Subtract(_startTime.Value);
            _secondsElapsed = (float)ts.TotalSeconds;
        }

        public void FinishRows(out UsageRow overflow)
        {
            if (_secondsElapsed > 3600.0f)
            {
                DateTime oneHourLater = _startTimeUTC.AddMinutes(-_startTimeUTC.Minute)
                                                     .AddSeconds(-_startTimeUTC.Second)
                                                     .AddHours(1);

                overflow = new UsageRow(new UserUsage
                                            {
                                                AppNames = new string[] { },
                                                AppUsedNameHashCodes = _overflowFilteredApps.ToArray(),
                                                AppUsedSeconds = _overflowFilteredSeconds.ToArray(),
                                                Start = oneHourLater
                                            });
            }
            else
            {
                overflow = null;

                float secondsTotal = _filteredSeconds.Sum();
                if (secondsTotal < 3600.0f)
                {
                    Log(noActivityApp, 3600.0f - secondsTotal);
                }
            }
        }

        public void Log(uint currentApp, float currentAppTime)
        {
            float prevSecondsElapsed = _secondsElapsed;
            
            // I've seen erroneous data, where currentAppTime is very large. It should never be more than 300 seconds.
            _secondsElapsed += Math.Min(300.0f, currentAppTime);

            if (_secondsElapsed > 3600.0f)
            {
                if (prevSecondsElapsed < 3600.0f)
                {
                    float after = _secondsElapsed - 3600.0f;
                    float before = currentAppTime - after;

                    // Half overflow
                    _overflowFilteredApps.Add(currentApp);
                    _overflowFilteredSeconds.Add(after);

                    // Half .. underflow
                    _filteredApps.Add(currentApp);
                    _filteredSeconds.Add(before);
                }
                else
                {
                    AddOrIncrementExisting(_overflowFilteredApps, _overflowFilteredSeconds, currentApp, currentAppTime);
                }
            }
            else
            {
                AddOrIncrementExisting(_filteredApps, _filteredSeconds, currentApp, currentAppTime);
            }
        }

        static void AddOrIncrementExisting(List<uint> apps, List<float> seconds, uint currentApp, float currentAppTime)
        {
            int count = apps.Count;
            if (count > 0 && apps[count - 1] == currentApp)
            {
                seconds[count - 1] += currentAppTime;
            }
            else
            {
                apps.Add(currentApp);
                seconds.Add(currentAppTime);
            }
        }
    }                               

}