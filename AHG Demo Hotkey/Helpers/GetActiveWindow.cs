using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace AHG_Demo_Hotkey.Helpers {
    class GetActiveWindow {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);
        [DllImport("user32.dll")]
        static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        public string GetWindowTitle() {

            const int nChars = 256;
            IntPtr handle = (IntPtr)0;
            StringBuilder Buff = new StringBuilder(nChars);

            handle = GetForegroundWindow();

            if (GetWindowText((int)handle, Buff, nChars) > 0) {
                return Buff.ToString();
                //return handle.ToString();
            } else {
                return "";
            }

        }
        public string GetProcessName() {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName.ToString();
        }
    }
}
