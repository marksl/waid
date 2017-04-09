using System.Threading;
using System.Windows;

namespace Waid
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Logger.Init();
            Config.Init();

            var transporter = new Transporter();

            new Thread(new Collector(transporter).Start) {IsBackground = true}.Start();
            new Thread(transporter.Start) { IsBackground = true}.Start();

            base.OnStartup(e);
        }
    }
}