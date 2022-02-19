using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AspGen
{
    public partial class QueryFormDimensions
    {
        // this code taked from
        //
        // Source: http://www.pinvoke.net/default.aspx/user32/getwindowrect.html

        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public struct TrueBounds
        {
            public Point P0;
            public Point P1;
            public Size RSize;
        }

        [Flags]
        private enum DwmWindowAttribute : uint
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        }

        public static TrueBounds GetWindowRectangle(IntPtr hWnd)
        {
            RECT rect;

            int size = Marshal.SizeOf(typeof(RECT));
            DwmGetWindowAttribute(hWnd, (int)DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out rect, size);

            TrueBounds tb;
            tb.P0 = new Point(rect.Left, rect.Top);
            tb.RSize = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
            tb.P1 = new Point(rect.Right, rect.Bottom);
            return tb;
        }
    }


}
