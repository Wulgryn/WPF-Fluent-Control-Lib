using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace WPF_Fluent_Control_Lib.SpecialBackground
{
    public enum AccentState
    {
        ACCENT_DISABLED = 0,
        /// <summary>
        /// One color background
        /// </summary>
        ACCENT_ENABLE_GRADIENT = 1,
        /// <summary>
        /// One color background with transparent
        /// </summary>
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        /// <summary>
        /// Simple default color blur
        /// </summary>
        ACCENT_ENABLE_BLURBEHIND = 3,
        /// <summary>
        /// Colored blur
        /// </summary>
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        /// <summary>
        /// No background
        /// </summary>
        ACCENT_INVALID_STATE = 5
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public uint AccentFlags;
        public uint GradientColor;
        public uint AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
    internal enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19
    }

    public class SpecialWindowBackground
    {
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        /// <summary>
        /// Enables the accent background. <br/>
        /// Needs to be run after the window is visible. (Loaded event)
        /// </summary>
        /// <param name="window">The window to set on the background.</param>
        /// <param name="accentState">The accent state.</param>
        /// <param name="TintOpacity">The opacity of the set color. (Max 100)</param>
        /// <param name="color">The color of the tint.</param>
        public static void Enable(Window window, AccentState accentState,int TintOpacity = 50, Color? color = null)
        {
            color = color ?? Colors.Black;
            var windowHelper = new WindowInteropHelper(window);
            nint hwnd = windowHelper.Handle;

            var accent = new AccentPolicy();
            accent.AccentState = accentState;
            accent.GradientColor = (uint)((TintOpacity << 24) | (((uint)color.Value.A << 24) | ((uint)color.Value.B << 16) | ((uint)color.Value.G << 8) | color.Value.R) & 0xFFFFFF); /*(White mask 0xFFFFFF)*/

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData();
            data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            SetWindowCompositionAttribute(hwnd, ref data);

            int attribute = DWMWA_WINDOW_CORNER_PREFERENCE;
            int preference = DWMWCP_ROUND;
            DwmSetWindowAttribute(hwnd, attribute, ref preference, sizeof(int));

            Marshal.FreeHGlobal(accentPtr);
        }
    }
}
