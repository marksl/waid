using System;
using System.Collections.Generic;
using System.Linq;
using Waid.WindowsAzure;

namespace Waid
{
    public class CollectorLog : ICollectorLog
    {
        private readonly Dictionary<string, uint> _appNames;
        private readonly List<float> _appUsedTimes;
        private readonly List<uint> _appsUsed;
        private readonly ITransporter _transporter;

        private DateTime _startTime;

        public CollectorLog(ITransporter transporter)
        {
            _transporter = transporter;
            _appsUsed = new List<uint>(200);
            _appUsedTimes = new List<float>(200);
            _appNames = new Dictionary<string, uint>(200);
        }

        public void Start(DateTime startTime)
        {
            _startTime = startTime;
            _appsUsed.Clear();
            _appUsedTimes.Clear();
            _appNames.Clear();

            Logger.Debug("Started: " + DateTime.UtcNow);
        }

        public void Log(string action, float seconds)
        {
            Logger.Debug(action + ": " + seconds);

            uint hash;
            if (!_appNames.TryGetValue(action, out hash))
            {
                hash = StringUtils.HashString(action);
                _appNames.Add(action, hash);
            }

            _appsUsed.Add(hash);
            _appUsedTimes.Add(seconds);
        }


        public void Send(DateTime now)
        {
            var userSettings = Properties.Settings.Default.UserId;
            if (userSettings == null || userSettings.Trim() == string.Empty)
            {
                Logger.Error("UserId is null!!!!");
                return;
            }
            var userId = new Guid(userSettings);

            var usage = new UserUsage
                            {
                                Start = _startTime,
                                AppNames = _appNames.Keys.ToArray(),
                                AppUsedNameHashCodes = _appsUsed.ToArray(),
                                AppUsedSeconds = _appUsedTimes.ToArray(),
                                UserId = userId
                            };

            _transporter.SendAsync(usage);

            Logger.DebugFormat(
                "Send - [_appNames:{0}] [_appsUsed : {1}] [_appUsedTimes: {2}] [_appUsedTimes.Sum(): {3}]",
                _appNames.Count, _appsUsed.Count, _appUsedTimes.Count, _appUsedTimes.Sum());

            Logger.LogUsage(usage);

            Start(now);
        }
    }
}