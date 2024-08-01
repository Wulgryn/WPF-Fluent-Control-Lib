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
    public class WindowEffectsHelper
    {
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public static void Enable(Window window)
        {
            var windowHelper = new WindowInteropHelper(window);
            IntPtr hwnd = windowHelper.Handle;

            int attribute = DWMWA_WINDOW_CORNER_PREFERENCE;
            int preference = DWMWCP_ROUND;
            DwmSetWindowAttribute(hwnd, attribute, ref preference, sizeof(int));
        }

        public static void Disable(Window window)
        {
            var windowHelper = new WindowInteropHelper(window);
            IntPtr hwnd = windowHelper.Handle;

            int attribute = DWMWA_WINDOW_CORNER_PREFERENCE;
            int preference = 0;
            DwmSetWindowAttribute(hwnd, attribute, ref preference, sizeof(int));
        }
    }
}
