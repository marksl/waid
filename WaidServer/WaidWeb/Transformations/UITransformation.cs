using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Waid.WindowsAzure;
using WaidWeb.Models;

namespace WaidWeb.Transformations
{
    internal class UITransformation
    {
        

        public static Data GetUIUsage(DailyUsage usage)
        {
            var idToName = new Dictionary<uint, string>();
            var idToColor = new Dictionary<uint, string>();
            var idToLegend = new Dictionary<uint, bool>();

            var appTimes = new Dictionary<string, double>();
            foreach (string appName in usage.AppNames)
            {
                uint hash = StringUtils.HashString(appName);

                idToName[hash] = appName;
                idToColor[hash] = ColorGenerator.GetColor(hash);
                idToLegend[hash] = true;
                appTimes[appName] = 0.0f;
            }

            if (usage.Hours.Length != 24)
            {
                throw new DataMisalignedException(
                    string.Format("There should be 24 hours of data, but there are [{0}] instead.",
                                  usage.Hours.Length));
            }

            var hourly = new List<HourlyData>[24];


            for (int i = 0; i < 24; i++)
            {
                var thisHour = new List<HourlyData>();
                HourlyUsage hour = usage.Hours[i];
                int count = hour.AppUsedSeconds.Length;

                for (int j = 0; j < count; j++)
                {
                    uint appUsed = hour.AppUsedNameHashCodes[j];
                    double secondsUsed = hour.AppUsedSeconds[j];

                    string name = idToName[appUsed];
                    string color = idToColor[appUsed];
                    bool legend = idToLegend[appUsed];

                    if (legend)
                    {
                        idToLegend[appUsed] = false;
                    }

                    appTimes[name] += secondsUsed;

                    thisHour.Add(new HourlyData(name, legend, color, secondsUsed));
                }

                hourly[i] = thisHour;
            }


            double maxVal = 0.0f;
            string maxKey = null;

            appTimes.Remove(UsageRepository.noActivity);

            List<string> keys = appTimes.Keys.ToList();
            double totalSeconds = appTimes.Values.Sum();
            foreach (var key in keys)
            {
                var val = (float)Math.Round(appTimes[key] / totalSeconds, 2);
                if (val > maxVal)
                {
                    maxVal = val;
                    maxKey = key;
                }
                appTimes[key] = val * 100;
            }

            double diff = 100.0 - appTimes.Values.Sum();
            if (maxKey != null)
            {
                appTimes[maxKey] += Math.Round(diff, 2);
            }

            // TODO: normalize the summary values.
            var summary = appTimes.Keys
                                  .Where(key => key != UsageRepository.noActivity)
                                  .Select(key => new SummaryData(key, appTimes[key])).ToList();

            return new Data
            {
                HourlyUsage = hourly,
                Summary = summary
            };
        }
 
    }
}