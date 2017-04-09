using System;
using System.Collections.Generic;
using Waid.WindowsAzure;

namespace WaidWeb.Transformations
{
    internal struct DailyTransformation
    {
        public static DailyUsage GetUsage(List<UsageRow> usageRows, int minutesOffset)
        {
            var hourBuckets = GetUsageGroupedByHour(usageRows, minutesOffset);
            var appNames = GetAppNames(usageRows);

            var hours = new List<HourlyUsage>();
            for (int i = 0; i < 24; i++)
            {
                List<UsageRow> rows = hourBuckets[i];
                
                if (rows.Count == 0)
                {
                    hours.Add(NoHourlyUsage);
                }
                else
                {
                    UsageRow overflow;
                    
                    HourlyUsage hourlyUsage = rows.ToHourlyUsage(minutesOffset, out overflow);
                    hours.Add(hourlyUsage);

                    if (overflow != null)
                    {
                        int nextBucket = i + 1;
                        if (nextBucket < 24)
                        {
                            hourBuckets[nextBucket].Insert(0, overflow);
                        }
                    }
                }
            }

            return new DailyUsage
            {
                AppNames = new List<string>(appNames),
                Hours = hours.ToArray()
            };
        }

        private static IEnumerable<string> GetAppNames(IEnumerable<UsageRow> usageRows)
        {
            var appNames = new HashSet<string> {UsageRepository.noActivity};
            // Sort rows in to buckets based on the hours
            foreach (UsageRow row in usageRows)
            {
                AddToAppNames(row, appNames);
            }
            return appNames;
        }

        private static List<UsageRow>[] GetUsageGroupedByHour(IEnumerable<UsageRow> usageRows, int minutesOffset)
        {
            var hourBuckets = new List<UsageRow>[24];
            for (int i = 0; i < 24; i++)
            {
                hourBuckets[i] = new List<UsageRow>();
            }

            // Sort rows in to buckets based on the hours
            foreach (UsageRow row in usageRows)
            {
                hourBuckets[row.GetStartTime(minutesOffset).Hour].Add(row);
            }
            return hourBuckets;
        }

        private static HourlyUsage NoHourlyUsage
        {
            get { return new HourlyUsage { AppUsedSeconds = new float[] { }, AppUsedNameHashCodes = new uint[] { } }; }
        }

        private static void AddToAppNames(UsageRow row, HashSet<string> appNames)
        {
            foreach (string appName in row.AppNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                appNames.Add(appName);
            }
        }

    }
}