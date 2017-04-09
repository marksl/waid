using System.Configuration;

namespace Waid
{
    public static class Config
    {
        private static double _timeBetweenSending;

        public static double TimeBetweenSending
        {
            get { return _timeBetweenSending; }
        }

        public static int Timeout { get; private set; }
        public static string Urls { get { return ConfigurationManager.AppSettings["Urls"];} }

        public static void Init()
        {
            string timeoutAppSetting = ConfigurationManager.AppSettings["timeout"];
            string timeBetweenSendingAppSetting = ConfigurationManager.AppSettings["timeBetweenSending"];

            if (!double.TryParse(timeBetweenSendingAppSetting, out _timeBetweenSending))
            {
                _timeBetweenSending = 30.0;
            }

            int timeout;
            if (!int.TryParse(timeoutAppSetting, out timeout))
            {
                Timeout = 500;
            }
        }
         
    }
}