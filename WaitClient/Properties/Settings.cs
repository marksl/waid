using System.Text;

namespace Waid.Properties
{
    partial class Settings
    {
        internal bool Sunday
        {
            get { return DaysToRun.Contains("sun"); }
        }

        internal bool Monday
        {
            get { return DaysToRun.Contains("mon"); }
        }

        internal bool Tues
        {
            get { return DaysToRun.Contains("tues"); }
        }

        internal bool Weds
        {
            get { return DaysToRun.Contains("weds"); }
        }

        internal bool Thurs
        {
            get { return DaysToRun.Contains("thurs"); }
        }

        internal bool Fri
        {
            get { return DaysToRun.Contains("fri"); }
        }

        internal bool Sat
        {
            get { return DaysToRun.Contains("sat"); }
        }

        internal void SetDays(bool sun, bool mon, bool tues, bool weds, bool thurs, bool fri, bool sat)
        {
            var builder = new StringBuilder();

            if (sun) builder.Append("sun ");
            if (mon) builder.Append("mon ");
            if (tues) builder.Append("tues ");
            if (weds) builder.Append("weds ");
            if (thurs) builder.Append("thurs ");
            if (fri) builder.Append("fri ");
            if (sat) builder.Append("sat");

            DaysToRun = builder.ToString();
        }
    }
}