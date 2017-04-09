using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Waid.WindowsAzure;

namespace WaidWeb.Transformations
{
    public class HourlyTransformations
    {
        public static HourlyUsage GetHourlyUsage(List<UsageRow> hourlyUsageRows, int minutesOffset, out UsageRow overflow)
        {
            var results = new HourlyTransformationResults(minutesOffset);

            // Create 1 buffer for each hour. This reduces the number of memory allocations.
            int usageInTotalBytes = GetUsageInSecondsTotal(hourlyUsageRows);
            var hashCodes = new uint[usageInTotalBytes/4];
            var times = new float[usageInTotalBytes/4];

            int destOffset = 0;

            foreach (UsageRow row in hourlyUsageRows)
            {
                var count = row.Apps.Length;
                Buffer.BlockCopy(row.UsageInSeconds, 0, times, destOffset, count);
                Buffer.BlockCopy(row.Apps, 0, hashCodes, destOffset, count);

                results.StartRow(row);

                int start = destOffset/4;
                int end = start + (count/4);

                for (int i = start; i < end; i++)
                {
                    uint currentApp = hashCodes[i];
                    float currentAppTime = times[i];

                    results.Log(currentApp, currentAppTime);
                }

                destOffset += count;
            }

            results.FinishRows(out overflow);

            return results.ToHourlyUsage();
        }

        private static int GetUsageInSecondsTotal(IList<UsageRow> hourlyUsageRows)
        {
            int usageInSecondsTotal = hourlyUsageRows.Sum(row => row.UsageInSeconds.Length);
            int usageAppsTotal = hourlyUsageRows.Sum(row => row.Apps.Length);
            if (usageInSecondsTotal != usageAppsTotal)
            {
                throw new InvalidDataException(string.Format("Usage:{0} and Rows:{1} should have the same length.",
                                                             usageInSecondsTotal, usageAppsTotal));
            }
            return usageInSecondsTotal;
        }

        
    }
}