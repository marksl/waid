using System;
using System.Collections.Generic;
using System.Globalization;

namespace WaidWeb.Models
{
    public class AppUsage
    {
        public string App { get; set; }
        public int UsageCount { get; set; }
        public double UsageTime { get; set; }
    }

    public class AggregateData
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public string App { get; set; }
        public List<AppUsage> Usage { get; set; }

        internal DateTime StartTime
        {
            get { return DateTime.Parse(RowKey); }
            set { RowKey = value.ToString(CultureInfo.InvariantCulture); }
        }

        internal string UserId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }
    }
}