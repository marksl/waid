using System;
using System.Collections.Generic;
using System.Globalization;
using Waid.WindowsAzure;
using WaidWeb.Controllers;
using WaidWeb.Models;

namespace WaidWeb.Transformations
{
    public class TableTransformation
    {
        public static IEnumerable<TableData> ToTableData(List<UsageRow> dataRows)
        {
            var mapping = new Dictionary<uint, string>();

            foreach (UsageRow row in dataRows)
            {
                foreach (var str in row.AppNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var hash = StringUtils.HashString(str);
                    if (!mapping.ContainsKey(hash))
                    {
                        mapping.Add(hash, str);
                    }
                }

                // This is potentially duplicated of HourlyTransformations.
                var timeUsed = new float[row.UsageInSeconds.Length / 4];
                Buffer.BlockCopy(row.UsageInSeconds, 0, timeUsed, 0, row.UsageInSeconds.Length);

                var appsUsed = new uint[row.Apps.Length / 4];
                Buffer.BlockCopy(row.Apps, 0, appsUsed, 0, row.Apps.Length);

                var appsUsedString = new string[appsUsed.Length];
                for (int i = 0; i < appsUsed.Length; i++)
                {
                    appsUsedString[i] = mapping[appsUsed[i]];
                }

                yield return new TableData
                {
                    apps = appsUsedString,
                    appTimes = timeUsed,
                    startDate = row.RowKey,
                    startDateString = row.GetStartTimeUtc().ToString(CultureInfo.InvariantCulture)
                };
            }
        } 
    }
}