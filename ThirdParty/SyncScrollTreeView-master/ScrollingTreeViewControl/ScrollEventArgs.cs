using System;

namespace devio.Windows.Controls
{
    public class ScrollEventArgs : EventArgs
    {
        private ScrollInfoStruct _si;

        public ScrollEventArgs()
        {
        }

        public ScrollInfoStruct ScrollInfo
        {
            get
            {
                return _si;
            }
            internal set
            {
                _si = value;
            }
        }
    }

    public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);
}
