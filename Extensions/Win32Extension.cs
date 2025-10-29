using System;
using System.Runtime.InteropServices;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.WindowsAPI;


namespace FLaUIDemo.Extensions
{
    
    public static class Win32Extension
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        /// <summary>
        /// Converts element client coordinates to screen coordinates.
        /// </summary>
        public static bool ClientToScreen(this AutomationElement element, ref POINT lpPoint)
        {
            IntPtr hWnd = element.Properties.NativeWindowHandle.Value;
            return ClientToScreen(hWnd, ref lpPoint);
        }

        /// <summary>
        /// Posts window message to the specified element.
        /// </summary>
        public static bool PostMessage(this AutomationElement element, uint Msg, IntPtr wParam, IntPtr lParam) {
            IntPtr hWnd = element.Properties.NativeWindowHandle.Value;
            return PostMessage(hWnd, Msg, wParam, lParam);
        }

        /// <summary>
        /// Converts screen coordinates to client coordinates relative to element.
        /// </summary>
        public static bool ScreenToClient(this AutomationElement element, ref POINT lpPoint){
            IntPtr hWnd = element.Properties.NativeWindowHandle.Value;
            return ScreenToClient(hWnd, ref lpPoint);
        }

    }
}
