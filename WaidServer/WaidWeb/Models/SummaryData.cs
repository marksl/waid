using System.Runtime.Serialization;
using Waid.WindowsAzure;
using WaidWeb.Transformations;

namespace WaidWeb.Models
{
    [DataContract]
    public class SummaryData
    {
        public SummaryData()
        {
        }

        public SummaryData(string name, double y)
            : this()
        {
            this.AppName = name;
            this.TimeInSeconds = y;
            this.Color = ColorGenerator.GetColor(StringUtils.HashString(name));
        }

        [DataMember(Name = "name")]
        public string AppName { get; set; }

        [DataMember(Name = "y")]
        public double TimeInSeconds { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }
    }
}