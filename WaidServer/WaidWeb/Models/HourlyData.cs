using System.Drawing;

namespace WaidWeb.Models
{
    public class HourlyData
    {
        public HourlyData()
        {
        }

        public HourlyData(string name, bool showInLegend, string htmlColor, double data)
        {
            this.name = name;
            this.showInLegend = showInLegend;
            color = htmlColor;
            this.data = new[] {data};
        }


        public HourlyData(string name, bool showInLegend, Color color, float data)
            : this(name, showInLegend, ColorTranslator.ToHtml(color), data)
        {
        }

        public string name { get; set; }
        public bool showInLegend { get; set; }
        public string color { get; set; }

        //each of these include only 1 value
        public double[] data { get; set; }
    }
}