
namespace DROMsM.Forms
{
    partial class LaunchBoxPlatformsForm
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
            this.olvLaunchBoxPlatformsListView = new BrightIdeasSoftware.FastObjectListView();
            this.olvNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvVisibleColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvFilePathColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howDoesThisWorkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.olvLaunchBoxPlatformsListView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvLaunchBoxPlatformsListView
            // 
            this.olvLaunchBoxPlatformsListView.AllColumns.Add(this.olvNameColumn);
            this.olvLaunchBoxPlatformsListView.AllColumns.Add(this.olvVisibleColumn);
            this.olvLaunchBoxPlatformsListView.AllColumns.Add(this.olvFilePathColumn);
            this.olvLaunchBoxPlatformsListView.AllowColumnReorder = true;
            this.olvLaunchBoxPlatformsListView.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
            this.olvLaunchBoxPlatformsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvLaunchBoxPlatformsListView.CellEditUseWholeCell = false;
            this.olvLaunchBoxPlatformsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvNameColumn,
            this.olvVisibleColumn,
            this.olvFilePathColumn});
            this.olvLaunchBoxPlatformsListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvLaunchBoxPlatformsListView.FullRowSelect = true;
            this.olvLaunchBoxPlatformsListView.GridLines = true;
            this.olvLaunchBoxPlatformsListView.HideSelection = false;
            this.olvLaunchBoxPlatformsListView.Location = new System.Drawing.Point(9, 24);
            this.olvLaunchBoxPlatformsListView.Margin = new System.Windows.Forms.Padding(0);
            this.olvLaunchBoxPlatformsListView.Name = "olvLaunchBoxPlatformsListView";
            this.olvLaunchBoxPlatformsListView.ShowGroups = false;
            this.olvLaunchBoxPlatformsListView.Size = new System.Drawing.Size(1373, 933);
            this.olvLaunchBoxPlatformsListView.TabIndex = 5;
            this.olvLaunchBoxPlatformsListView.TintSortColumn = true;
            this.olvLaunchBoxPlatformsListView.UseAlternatingBackColors = true;
            this.olvLaunchBoxPlatformsListView.UseCompatibleStateImageBehavior = false;
            this.olvLaunchBoxPlatformsListView.UseFilterIndicator = true;
            this.olvLaunchBoxPlatformsListView.UseFiltering = true;
            this.olvLaunchBoxPlatformsListView.UseHotControls = false;
            this.olvLaunchBoxPlatformsListView.View = System.Windows.Forms.View.Details;
            this.olvLaunchBoxPlatformsListView.VirtualMode = true;
            this.olvLaunchBoxPlatformsListView.SubItemChecking += new System.EventHandler<BrightIdeasSoftware.SubItemCheckingEventArgs>(this.olvLaunchBoxPlatformsListView_SubItemChecking);
            // 
            // olvNameColumn
            // 
            this.olvNameColumn.AspectName = "Name";
            this.olvNameColumn.MinimumWidth = 341;
            this.olvNameColumn.Text = "Platform";
            this.olvNameColumn.UseFiltering = false;
            this.olvNameColumn.Width = 341;
            // 
            // olvVisibleColumn
            // 
            this.olvVisibleColumn.AspectName = "Visible";
            this.olvVisibleColumn.CheckBoxes = true;
            this.olvVisibleColumn.MaximumWidth = 42;
            this.olvVisibleColumn.MinimumWidth = 42;
            this.olvVisibleColumn.Text = "Visible";
            this.olvVisibleColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvVisibleColumn.Width = 42;
            // 
            // olvFilePathColumn
            // 
            this.olvFilePathColumn.AspectName = "FilePath";
            this.olvFilePathColumn.FillsFreeSpace = true;
            this.olvFilePathColumn.MinimumWidth = 60;
            this.olvFilePathColumn.Text = "File";
            this.olvFilePathColumn.UseFiltering = false;
            this.olvFilePathColumn.Width = 80;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.operationsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1391, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // operationsToolStripMenuItem
            // 
            this.operationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllToolStripMenuItem,
            this.hideAllToolStripMenuItem});
            this.operationsToolStripMenuItem.Name = "operationsToolStripMenuItem";
            this.operationsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.operationsToolStripMenuItem.Text = "Operations";
            // 
            // showAllToolStripMenuItem
            // 
            this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            this.showAllToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.showAllToolStripMenuItem.Text = "Show All...";
            this.showAllToolStripMenuItem.Click += new System.EventHandler(this.showAllToolStripMenuItem_Click);
            // 
            // hideAllToolStripMenuItem
            // 
            this.hideAllToolStripMenuItem.Name = "hideAllToolStripMenuItem";
            this.hideAllToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.hideAllToolStripMenuItem.Text = "Hide All...";
            this.hideAllToolStripMenuItem.Click += new System.EventHandler(this.hideAllToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howDoesThisWorkToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howDoesThisWorkToolStripMenuItem
            // 
            this.howDoesThisWorkToolStripMenuItem.Name = "howDoesThisWorkToolStripMenuItem";
            this.howDoesThisWorkToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.howDoesThisWorkToolStripMenuItem.Text = "How does this work?";
            this.howDoesThisWorkToolStripMenuItem.Click += new System.EventHandler(this.howDoesThisWorkToolStripMenuItem_Click);
            // 
            // LaunchBoxPlatformsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1391, 966);
            this.Controls.Add(this.olvLaunchBoxPlatformsListView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LaunchBoxPlatformsForm";
            this.Text = "Manage Platforms - LaunchBox";
            ((System.ComponentModel.ISupportInitialize)(this.olvLaunchBoxPlatformsListView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView olvLaunchBoxPlatformsListView;
        private BrightIdeasSoftware.OLVColumn olvNameColumn;
        private BrightIdeasSoftware.OLVColumn olvFilePathColumn;
        private BrightIdeasSoftware.OLVColumn olvVisibleColumn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howDoesThisWorkToolStripMenuItem;
    }
}