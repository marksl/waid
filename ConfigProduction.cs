namespace Waid
{
    public static class Config
    {
        public static double TimeBetweenSending { get { return 30.0; } }
        public static int Timeout { get { return 500; } }
        public static string Urls { get { return "http://www.waidmonitor.com/"; } }
        public static void Init()
        {
            
        }
    }
}