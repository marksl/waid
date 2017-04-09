using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Waid.WindowsAzure;
using log4net;
using log4net.Config;

namespace Waid
{
    internal static class Logger
    {
        private static readonly ILog _log4Net = LogManager.GetLogger(typeof (CollectorLog));

        public static void Init()
        {
            XmlConfigurator.Configure();
        }

        public static void Debug(string debug)
        {
            _log4Net.Debug(debug);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            _log4Net.DebugFormat(format, args);
        }

        public static void Error(string error)
        {
            _log4Net.Error(error);
        }

        public static void LogUsage(UserUsage logUsage)
        {
            var mappings = new Dictionary<uint, string>();

            foreach (string app in logUsage.AppNames)
            {
                mappings[StringUtils.HashString(app)] = app;
            }

            using (FileStream fileStream = File.Open("compare-to-html.csv", FileMode.Append))
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(logUsage.Start.Ticks.ToString(CultureInfo.InvariantCulture));
                writer.Write(",");
                writer.Write(logUsage.Start.ToString(CultureInfo.InvariantCulture).Replace('"', '-'));
                writer.Write(',');
                writer.Write("AppName,Seconds");
                writer.WriteLine();

                for (int i = 0; i < logUsage.AppUsedNameHashCodes.Length; i++)
                {
                    var hashCode = logUsage.AppUsedNameHashCodes[i];
                    var seconds = logUsage.AppUsedSeconds[i];

                    writer.Write(",,,");
                    writer.Write(mappings[hashCode]);
                    writer.Write(',');
                    writer.Write(seconds);
                    writer.WriteLine();
                }
            }
        }

        public static void ErrorFormat(string format, params object[] parameters)
        {
            _log4Net.ErrorFormat(format, parameters);
}

        public static void Error(string message, Exception ex)
        {
            _log4Net.Error(message, ex);
        }
    }
}