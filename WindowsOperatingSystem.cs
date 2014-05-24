using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Waid
{
    public class WindowsOperatingSystem : IOperatingSystem
    {
        private NativeMethods.LASTINPUTINFO _info;
        
        public WindowsOperatingSystem()
        {
            _info = new NativeMethods.LASTINPUTINFO();
            _info.cbSize = (uint)Marshal.SizeOf(_info);
            _info.dwTime = 0;
        }

        public string GetCurrentProcessName()
        {
            IntPtr hwnd = NativeMethods.GetForegroundWindow();
            Int32 pid;
            NativeMethods.GetWindowThreadProcessId(hwnd, out pid);

            Process p = Process.GetProcessById(pid);
            string processName = p.ProcessName;

            Logger.DebugFormat("WindowsOperatingSystem - GetCurrentProcessName - [hwnd: {0}] [pid: {1}] [processName: {2}", hwnd, pid, processName);

            return processName;
        }

        public ulong GetMillisecondsSinceLastInput()
        {
            if (NativeMethods.GetLastInputInfo(ref _info))
            {
                ulong tickCount64 = NativeMethods.GetTickCount64();
                uint dwTime = _info.dwTime;
                ulong milliseconds = tickCount64 - dwTime;

                Logger.DebugFormat("WindowsOperatingSystem - GetMillisecondsSinceLastInput - [tickCount64: {0}] [dwTime:{1}] [milliseconds: {2}]",
                                     tickCount64, dwTime, milliseconds);
                return milliseconds;
            }

            Logger.Debug("WindowsOperatingSystem - GetMillisecondsSinceLastInput - false");

            return 0;
        }

    }
}