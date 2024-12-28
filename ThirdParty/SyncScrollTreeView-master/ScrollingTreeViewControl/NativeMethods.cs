using System;
using System.Runtime.InteropServices;

namespace devio.Windows.Controls
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScrollInfoStruct
    {
        public int cbSize;
        public int fMask;
        public int nMin;
        public int nMax;
        public int nPage;
        public int nPos;
        public int nTrackPos;
    }

    internal static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int GetScrollInfo(IntPtr hWnd, int n,
            ref ScrollInfoStruct lpScrollInfo);

        [DllImport("user32.dll")]
        internal static extern int SetScrollInfo(int hWnd, int n,
            [MarshalAs(UnmanagedType.Struct)] ref ScrollInfoStruct lpcScrollInfo,
            bool b);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        internal static extern int SendMessageLong(
            int hwnd,
            uint wMsg,
            int wParam,
            int lParam);

        // Win32 Const
        public const int SBM_SETSCROLLINFO = 0x00E9;
        public const int WM_HSCROLL = 0x114;
        public const int WM_VSCROLL = 0x115;
        public const int WM_MOUSEWHEEL = 0x20A;
        public const int SB_HORZ = 0;
        public const int SB_VERT = 1;
        public const int SB_THUMBTRACK = 5;

        public const int SIF_TRACKPOS = 0x10;
        public const int SIF_RANGE = 0x1;
        public const int SIF_POS = 0x4;
        public const int SIF_PAGE = 0x2;
        public const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
    }
}
