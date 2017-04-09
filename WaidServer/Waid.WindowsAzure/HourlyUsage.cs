using System;

namespace Waid.WindowsAzure
{
    public class UserUsage
    {
        public Guid UserId { get; set; }

        public DateTime Start { get; set; }

        public string[] AppNames { get; set; }

        public float[] AppUsedSeconds { get; set; }
        public uint[] AppUsedNameHashCodes { get; set; }
    }

    public class HourlyUsage
    {
        public float[] AppUsedSeconds { get; set; }
        public uint[] AppUsedNameHashCodes { get; set; }
    }
}