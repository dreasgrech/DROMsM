using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace devio.Windows.Controls
{
    // this assembly is based on
    // http://www.codegod.biz/WebAppCodeGod/TreeView-scrolling-synchronisieren-AID241.aspx
    // http://web.archive.org/web/20080921232227/http://codegod.de/WebAppCodeGod/TreeView-scrolling-synchronisieren-AID241.aspx

    public class ScrollTreeView : TreeView
    {
        public event ScrollEventHandler ScrollH;
        public event ScrollEventHandler ScrollV;

        protected virtual void OnScrollH(ScrollEventArgs e)
        {
            if (ScrollH != null)
            {
                ScrollH(this, e);
            }
        }

        protected virtual void OnScrollV(ScrollEventArgs e)
        {
            if (ScrollV != null)
            {
                ScrollV(this, e);
            }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {

                base.WndProc(ref m);

                switch (m.Msg)
                {
                    // Vertical scroll by Mouse
                    case NativeMethods.WM_VSCROLL:
                    case NativeMethods.SBM_SETSCROLLINFO:
                        RaiseScrollVEvent(m.HWnd);
                        break;

                    case NativeMethods.WM_HSCROLL:
                        RaiseScrollHEvent(m.HWnd);
                        break;

                    case NativeMethods.WM_MOUSEWHEEL:
                        RaiseScrollVEvent(m.HWnd);
                        RaiseScrollHEvent(m.HWnd);
                        break;
                }
            }
            catch (Exception ex)
            {
                // swallow
            }
        }

        /// <summary>
        /// Perhaps scrolled by keystroke 
        /// (optimize that for up, down etc.)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            RaiseScrollVEvent(this.Handle);
        }

        /// <summary>
        /// Raise event that scrolling has occured
        /// </summary>
        /// <param name="handle"></param>
        private void RaiseScrollVEvent(IntPtr handle)
        {
            ScrollInfoStruct si = new ScrollInfoStruct();
            si.fMask = NativeMethods.SIF_ALL;
            si.cbSize = Marshal.SizeOf(si);

            NativeMethods.GetScrollInfo(handle, NativeMethods.SB_VERT, ref si);

            ScrollEventArgs e = new ScrollEventArgs();
            e.ScrollInfo = si;

            OnScrollV(e);
        }

        private void RaiseScrollHEvent(IntPtr handle)
        {
            ScrollInfoStruct si = new ScrollInfoStruct();
            si.fMask = NativeMethods.SIF_ALL;
            si.cbSize = Marshal.SizeOf(si);

            NativeMethods.GetScrollInfo(handle, NativeMethods.SB_HORZ, ref si);

            ScrollEventArgs e = new ScrollEventArgs();
            e.ScrollInfo = si;

            OnScrollH(e);
        }

        /// <summary>
        /// Scroll to a vertical position
        /// </summary>
        /// <param name="e"></param>
        public void ScrollToPositionV(ScrollInfoStruct si)
        {
            ScrollInfoStruct siOwn = new ScrollInfoStruct();
            siOwn.fMask = NativeMethods.SIF_ALL;
            siOwn.cbSize = Marshal.SizeOf(si);

            NativeMethods.GetScrollInfo(this.Handle, NativeMethods.SB_VERT, ref siOwn);

            if (siOwn.nPos != si.nPos)
            {
                NativeMethods.SetScrollInfo(this.Handle.ToInt32(), NativeMethods.SB_VERT, ref si, true);
                NativeMethods.SendMessageLong(this.Handle.ToInt32(), NativeMethods.WM_VSCROLL,
                    NativeMethods.SB_THUMBTRACK + 0x10000 * si.nPos, 0);
            }
        }

        /// <summary>
        /// Scroll to a vertical position
        /// </summary>
        /// <param name="e"></param>
        public void ScrollToPositionH(ScrollInfoStruct si)
        {
            ScrollInfoStruct siOwn = new ScrollInfoStruct();
            siOwn.fMask = NativeMethods.SIF_ALL;
            siOwn.cbSize = Marshal.SizeOf(si);

            NativeMethods.GetScrollInfo(this.Handle, NativeMethods.SB_HORZ, ref siOwn);

            if (siOwn.nPos != si.nPos)
            {
                NativeMethods.SetScrollInfo(this.Handle.ToInt32(), NativeMethods.SB_HORZ, ref si, true);
                NativeMethods.SendMessageLong(this.Handle.ToInt32(), NativeMethods.WM_HSCROLL,
                    NativeMethods.SB_THUMBTRACK + 0x10000 * si.nPos, 0);
            }
        }
    }
}
