using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace WPF_Fluent_Control_Lib.Helpers
{
    public class MonitorHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private const uint MONITORINFOF_PRIMARY = 0x00000001;

        // Import the necessary functions from user32.dll
        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        public static RECT GetWorkMonitorInfo(Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);
            IntPtr hwnd = windowInteropHelper.Handle;

            // Get the monitor handle for the window
            IntPtr hMonitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            // Initialize MONITORINFO structure
            MONITORINFO mi = new MONITORINFO { cbSize = (uint)Marshal.SizeOf(typeof(MONITORINFO)) };

            // Get monitor information
            if (GetMonitorInfo(hMonitor, ref mi))
            {
                var monitorBounds = mi.rcWork;
                //MessageBox.Show($"Monitor size: {monitorBounds.right - monitorBounds.left}x{monitorBounds.bottom - monitorBounds.top}\n" +
                //                $"Monitor position: X={monitorBounds.left}, Y={monitorBounds.top}", "Monitor Info");
                return mi.rcWork;
            }
            else
            {
                MessageBox.Show("Failed to get monitor information.", "Error");
                return new RECT();
            }
        }

        public static RECT GetMonitorInfo(Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);
            IntPtr hwnd = windowInteropHelper.Handle;

            // Get the monitor handle for the window
            IntPtr hMonitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            // Initialize MONITORINFO structure
            MONITORINFO mi = new MONITORINFO { cbSize = (uint)Marshal.SizeOf(typeof(MONITORINFO)) };

            // Get monitor information
            if (GetMonitorInfo(hMonitor, ref mi))
            {
                var monitorBounds = mi.rcWork;
                //MessageBox.Show($"Monitor size: {monitorBounds.right - monitorBounds.left}x{monitorBounds.bottom - monitorBounds.top}\n" +
                //                $"Monitor position: X={monitorBounds.left}, Y={monitorBounds.top}", "Monitor Info");
                return mi.rcMonitor;
            }
            else
            {
                MessageBox.Show("Failed to get monitor information.", "Error");
                return new RECT();
            }
        }
    }
}
