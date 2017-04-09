using System.Collections.Generic;

namespace Waid.WindowsAzure
{
    public class DailyUsage
    {
        public List<string> AppNames { get; set; }
        public HourlyUsage[] Hours { get; set; }
    }
}