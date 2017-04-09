using System.Collections.Generic;

namespace WaidWeb.Models
{
    public class Data
    {
        public List<SummaryData> Summary { get; set; }

        public List<HourlyData>[] HourlyUsage { get; set; }
    }
}