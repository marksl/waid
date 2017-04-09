using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Timer = System.Timers.Timer;

namespace Service
{
    public partial class Service1 : ServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Service1));

        public const string ServiceNameShared = "POC Service";
        public Service1()
        {
            ServiceName = ServiceNameShared;
        }


        protected override void OnStart(string[] args)
        {
            

            new Thread(HiddenForm.Create).Start();
        }

        protected override void OnStop()
        {
        }


    }
}
