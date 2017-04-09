using System;
using Waid.WindowsAzure;

namespace Waid
{
    internal static class Logger
    {
        // Hmmm... Im starting to re-think this 'no logggin in production' idea..

        public static void Init()
        {
        }

        public static void Debug(string debug)
        {
        }

        public static void DebugFormat(string format, params object[] args)
        {
        }

        public static void Error(string error)
        {
        }

        public static void LogUsage(UserUsage logUsage)
        {
        }

        public static void ErrorFormat(string format, params object[] parameters)
        {
        }

        public static void Error(string message, Exception ex)
        {
        }
    }
}