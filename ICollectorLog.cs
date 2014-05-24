using System;

namespace Waid
{
    public interface ICollectorLog
    {
        void Start(DateTime startTime);
        void Log(string action, float seconds);
        void Send(DateTime now);
    }
}