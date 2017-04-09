using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using MouseKeyboardActivityMonitor;
//using MouseKeyboardActivityMonitor.WinApi;

//using Gma.UserActivityMonitor;

namespace ProofOfConcept
{
    class Program
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out System.Drawing.Point lpPoint);


        //This Function is used to get Active Window Title...
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hwnd, string lpString, int cch);

        //This Function is used to get Handle for Active Window...
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();

        //This Function is used to get Active process ID...
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out Int32 lpdwProcessId);

        public static string ActiveApplTitle()
        {
            //This method is used to get active application's title using GetWindowText() method present in user32.dll
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd.Equals(IntPtr.Zero)) return "";
            string lpText = new string((char)0, 100);
            int intLength = GetWindowText(hwnd, lpText, lpText.Length);
            if ((intLength <= 0) || (intLength > lpText.Length)) return "unknown";
            return lpText.Trim();
        }

        //static KeyboardHookListener m;
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("start");
            //m = new KeyboardHookListener(new GlobalHooker());
            //m.MouseMove += new MouseEventHandler(m_MouseMove);
            //m.Enabled = true;
            _hookID = SetHook(_proc);

            Console.WriteLine("starting");

            int i = 0;
            while( i++ < 30)
            {
                Point point = new Point(0,0);
                if(GetCursorPos(out point))
                {
                    
                }

                IntPtr hwnd = GetForegroundWindow();
                Int32 pid;
                GetWindowThreadProcessId(hwnd, out pid);

                Process p = Process.GetProcessById(pid);
                string appltitle = ActiveApplTitle().Trim().Replace("\0", "");

                lock (locker)
                {
                    Console.WriteLine(string.Format("{0} {1} {2} {3}", appltitle, point.X, point.Y, l));
                    l = string.Empty;
                }

                //Console.Out.WriteLine(string.Format("{0} {1} {2}", appltitle, x ,y));
                

                Thread.Sleep(1000);
            }
            UnhookWindowsHookEx(_hookID);
            //m.Dispose();
        }

        static private int x;
        static private int y;
        static void m_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
        }


        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                
                lock(locker)
                {
                    l = l + ((Keys) vkCode).ToString();
                }
                //Console.WriteLine((Keys)vkCode);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static object locker = new object();

        private static string l = string.Empty;


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int  WM_KEYUP                        = 0x0101;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;


        
    }
}
