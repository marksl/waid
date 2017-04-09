using System;
using System.Globalization;
using Microsoft.WindowsAzure.StorageClient;

namespace Waid.WindowsAzure
{
    public class UsageRow : TableServiceEntity
    {
        public UsageRow()
        {
        }

        public UsageRow(UserUsage usage)
            : base(usage.UserId.ToString(), usage.Start.Ticks.ToString(CultureInfo.InvariantCulture))
        {
            if (usage.AppUsedSeconds.Length != usage.AppUsedNameHashCodes.Length)
            {
                throw new ArgumentException(
                    string.Format(
                        "The AppUsedSeconds: {0} and AppUsedNameHashCodes: {1} should have the same number of elements.",
                        usage.AppUsedSeconds.Length, usage.AppUsedNameHashCodes.Length));
            }

            var byteArray = new byte[usage.AppUsedSeconds.Length*4];
            Buffer.BlockCopy(usage.AppUsedSeconds, 0, byteArray, 0, byteArray.Length);
            UsageInSeconds = byteArray;

            byteArray = new byte[usage.AppUsedSeconds.Length*4];
            Buffer.BlockCopy(usage.AppUsedNameHashCodes, 0, byteArray, 0, byteArray.Length);
            Apps = byteArray;

            AppNames = string.Join(",", usage.AppNames);
        }

        public string AppNames { get; set; }
        public byte[] UsageInSeconds { get; set; }
        public byte[] Apps { get; set; }

        public DateTime GetStartTimeUtc()
        {
            return new DateTime(Convert.ToInt64(RowKey));
        }

        public DateTime GetStartTime(int minuteOffset)
        {
            return GetStartTimeUtc().Subtract(new TimeSpan(0, minuteOffset, 0));
        }
    }
}