namespace devio.ScrollingTreeViewSample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tvRight = new devio.Windows.Controls.ScrollTreeView();
            this.tvLeft = new devio.Windows.Controls.ScrollTreeView();
            this.SuspendLayout();
            // 
            // tvRight
            // 
            this.tvRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tvRight.Location = new System.Drawing.Point(325, 12);
            this.tvRight.Name = "tvRight";
            this.tvRight.Size = new System.Drawing.Size(280, 431);
            this.tvRight.TabIndex = 1;
            this.tvRight.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvRight_AfterCollapse);
            this.tvRight.ScrollV += new devio.Windows.Controls.ScrollEventHandler(this.tvRight_ScrollV);
            this.tvRight.ScrollH += new devio.Windows.Controls.ScrollEventHandler(this.tvRight_ScrollH);
            this.tvRight.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvRight_AfterExpand);
            // 
            // tvLeft
            // 
            this.tvLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tvLeft.Location = new System.Drawing.Point(12, 12);
            this.tvLeft.Name = "tvLeft";
            this.tvLeft.Size = new System.Drawing.Size(280, 431);
            this.tvLeft.TabIndex = 0;
            this.tvLeft.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvLeft_AfterCollapse);
            this.tvLeft.ScrollV += new devio.Windows.Controls.ScrollEventHandler(this.tvLeft_ScrollV);
            this.tvLeft.ScrollH += new devio.Windows.Controls.ScrollEventHandler(this.tvLeft_ScrollH);
            this.tvLeft.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvLeft_AfterExpand);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 455);
            this.Controls.Add(this.tvRight);
            this.Controls.Add(this.tvLeft);
            this.Name = "Form1";
            this.Text = "devio ScrollingTreeView Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private devio.Windows.Controls.ScrollTreeView tvLeft;
        private devio.Windows.Controls.ScrollTreeView tvRight;
    }
}

