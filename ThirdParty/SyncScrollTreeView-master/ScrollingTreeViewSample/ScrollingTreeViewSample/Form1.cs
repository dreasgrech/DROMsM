using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace devio.ScrollingTreeViewSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var tnLeftRoot = tvLeft.Nodes.Add("root");
            var tnRightRoot = tvRight.Nodes.Add("root");
            tnLeftRoot.Tag = tnRightRoot;
            tnRightRoot.Tag = tnLeftRoot;

            TreeNode tnL = null, tnR = null;

            for (var i = 0; i < 40; i++)
            {
                if (i % 10 == 0)
                {
                    tnL = tnLeftRoot.Nodes.Add("this is node " + i);
                    tnR = tnRightRoot.Nodes.Add("this is node " + i);

                    tnL.Tag = tnR;
                    tnR.Tag = tnL;
                }
                else
                {
                    var tnLL = tnL.Nodes.Add("this is node " + i);
                    var tnRR = tnR.Nodes.Add("this is node " + i);
                    
                    tnLL.Tag = tnRR;
                    tnRR.Tag = tnLL;
                }
            }

            tnLeftRoot.ExpandAll();
            tnRightRoot.ExpandAll();
            tvLeft.TopNode = tnLeftRoot;
            tvRight.TopNode = tnRightRoot;

            Form1_Resize(null, null);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int width = ClientSize.Width - 32;
            tvLeft.Width = width / 2;
            tvRight.Left = tvLeft.Right + 8;
            tvRight.Width = width / 2;
        }

        private void tvLeft_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            (e.Node.Tag as TreeNode).Collapse();
        }

        private void tvLeft_AfterExpand(object sender, TreeViewEventArgs e)
        {
            (e.Node.Tag as TreeNode).Expand();
        }

        private void tvLeft_ScrollH(object sender, devio.Windows.Controls.ScrollEventArgs e)
        {
            tvRight.ScrollToPositionH(e.ScrollInfo);
        }

        private void tvLeft_ScrollV(object sender, devio.Windows.Controls.ScrollEventArgs e)
        {
            tvRight.ScrollToPositionV(e.ScrollInfo);
        }

        private void tvRight_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            (e.Node.Tag as TreeNode).Collapse();
        }

        private void tvRight_AfterExpand(object sender, TreeViewEventArgs e)
        {
            (e.Node.Tag as TreeNode).Expand();
        }

        private void tvRight_ScrollH(object sender, devio.Windows.Controls.ScrollEventArgs e)
        {
            tvLeft.ScrollToPositionH(e.ScrollInfo);
        }

        private void tvRight_ScrollV(object sender, devio.Windows.Controls.ScrollEventArgs e)
        {
            tvLeft.ScrollToPositionV(e.ScrollInfo);
        }

        private void ExpandSubtree(TreeNode tn)
        {
            foreach (TreeNode tnT in tn.Nodes)
            {
                tnT.Expand();
                if (tnT.Nodes.Count > 0)
                    ExpandSubtree(tnT);
            }
        }
    }
}
