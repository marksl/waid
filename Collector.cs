using System;
using System.Threading;

namespace Waid
{
    internal class Collector
    {
        private readonly IOperatingSystem _os;
        private readonly ICollectorLog _collectorLog;

        private string _currentProcess;
        private DateTime _currentProcessStartTime;
        private DateTime _lastSave;
        private ulong _millisecondsIdle;
        private double _timeBetweenSending;

        public Collector(ITransporter transporter)
            : this(new WindowsOperatingSystem(), new CollectorLog(transporter))
        {
        }

        private Collector(IOperatingSystem os, ICollectorLog collectorLog)
        {
            _os = os;
            _collectorLog = collectorLog;
            _currentProcess = os.GetCurrentProcessName();
            _currentProcessStartTime = DateTime.UtcNow;
            _lastSave = DateTime.UtcNow;
            _millisecondsIdle = 0;
        }
        
        public void Start()
        {
            _timeBetweenSending = Config.TimeBetweenSending;
            int timeout = Config.Timeout;
            
            _collectorLog.Start(_currentProcessStartTime);
            while (true)
            {
                if (WatchMe.IsNowAGoodTime())
                {
                    RunOnce(_os, _collectorLog);
                }

                Thread.Sleep(timeout);
            }
        }

        public void RunOnce(IOperatingSystem os, ICollectorLog collectorLog)
        {
            DateTime now = DateTime.UtcNow;

            ulong milliseconds = os.GetMillisecondsSinceLastInput();
            if (milliseconds > 15000)
            {
                _millisecondsIdle = milliseconds;
            }
            else
            {
                if (_millisecondsIdle != 0)
                {
                    collectorLog.Log("Idle", GetTimeInSeconds(now));

                    _currentProcessStartTime = now;
                    _millisecondsIdle = 0;
                    return;
                }
            }

            string processName = os.GetCurrentProcessName();
            if (processName != _currentProcess)
            {
                collectorLog.Log(_currentProcess, GetTimeInSeconds(now));

                _currentProcess = processName;
                _currentProcessStartTime = now;
            }

            // Every 10 minutes save
            if (now.Subtract(_lastSave).TotalSeconds > _timeBetweenSending)
            {
                string log = _millisecondsIdle > 15000 ? "Idle" : _currentProcess;

                collectorLog.Log(log, GetTimeInSeconds(now));
                _currentProcessStartTime = now;
                _lastSave = now;

                collectorLog.Send(now);
            }
        }

        private float GetTimeInSeconds(DateTime now)
        {
            return (float) now.Subtract(_currentProcessStartTime).TotalSeconds;
        }
    }
}