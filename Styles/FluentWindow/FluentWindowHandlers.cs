using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using WPF_Fluent_Control_Lib.Helpers;
using WPF_Fluent_Control_Lib.SpecialBackground;
using static WPF_Fluent_Control_Lib.Helpers.MonitorHelper;

namespace WPF_Fluent_Control_Lib.Styles.FluentWindow
{

    public static class FluentWindowHandlers
    {
        public static readonly RoutedEventHandler OnWindowLoaded = new RoutedEventHandler(WindowLoaded);
        private static Window _window;
        private static void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if(sender is Window window && window.IsLoaded)
            {
                _window = window;

                System.IntPtr handle = new WindowInteropHelper(window).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));

                Thickness border_thickness = window.BorderThickness;

                window.StateChanged += (s, e) =>
                {
                    if (window.WindowState == WindowState.Maximized)
                    {
                        border_thickness = window.BorderThickness;
                        window.BorderThickness = new Thickness(0);
                        WindowEffectsHelper.Disable(window);
                    }
                    else
                    {
                        window.BorderThickness = border_thickness;
                        WindowEffectsHelper.Enable(window);
                    }
                };
                System.Windows.Media.Color c = ((SolidColorBrush)window.Background).Color;
                SpecialWindowBackground.Enable(window, AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND, 50, c == Colors.Transparent ? null : c);
            }
        }

        private static System.IntPtr WindowProc(
              System.IntPtr hwnd,
              int msg,
              System.IntPtr wParam,
              System.IntPtr lParam,
              ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (System.IntPtr)0;
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {

            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            MONITORINFO monitorInfo = new MONITORINFO();
            RECT rcWorkArea = GetWorkMonitorInfo(_window);
            RECT rcMonitorArea = GetMonitorInfo(_window);
            mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
            mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
            mmi.ptMaxSize.X = Math.Abs(rcWorkArea.right - rcWorkArea.left);
            mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            Marshal.StructureToPtr(mmi, lParam, true);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }
    }
}
