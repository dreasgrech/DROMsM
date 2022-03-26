using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Frontend
{
    public class DATFileMachineVirtualListDataSource : FastObjectListDataSource
    {
        public DATFileMachineVirtualListDataSource(FastObjectListView listView) : base(listView)
        {
        }
    }
}