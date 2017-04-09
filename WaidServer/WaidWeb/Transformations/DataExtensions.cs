using System;
using System.Collections.Generic;
using Waid.WindowsAzure;
using WaidWeb.Models;

namespace WaidWeb.Transformations
{
    public static class DataExtensions
    {
        public static DailyUsage ToDaily(this List<UsageRow> usageRows,  int minutesOffset)
        {
            DailyUsage dailyUsage = DailyTransformation.GetUsage(usageRows, minutesOffset);
            return dailyUsage;
        }

        public static HourlyUsage ToHourlyUsage(this List<UsageRow> hourlyUsageRows, int minutesOffset,
                                                out UsageRow overflow)
        {
            var dailyUsage = HourlyTransformations.GetHourlyUsage(hourlyUsageRows, minutesOffset, out overflow);
            return dailyUsage;
        }

        public static Data ToUI(this DailyUsage usage)
        {
            return UITransformation.GetUIUsage(usage);
        }

        public static IEnumerable<TableData> ToTableData(this List<UsageRow> usageRows)
        {
            return TableTransformation.ToTableData(usageRows);
        }
    }
}