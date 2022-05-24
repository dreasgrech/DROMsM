
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
            ((System.ComponentModel.ISupportInitialize)(this.olvLaunchBoxPlatformsListView)).BeginInit();
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
            this.olvLaunchBoxPlatformsListView.Location = new System.Drawing.Point(9, 9);
            this.olvLaunchBoxPlatformsListView.Margin = new System.Windows.Forms.Padding(0);
            this.olvLaunchBoxPlatformsListView.Name = "olvLaunchBoxPlatformsListView";
            this.olvLaunchBoxPlatformsListView.ShowGroups = false;
            this.olvLaunchBoxPlatformsListView.Size = new System.Drawing.Size(1373, 948);
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
            // LaunchBoxPlatformsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1391, 966);
            this.Controls.Add(this.olvLaunchBoxPlatformsListView);
            this.Name = "LaunchBoxPlatformsForm";
            this.Text = "Manage Platforms - LaunchBox";
            ((System.ComponentModel.ISupportInitialize)(this.olvLaunchBoxPlatformsListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView olvLaunchBoxPlatformsListView;
        private BrightIdeasSoftware.OLVColumn olvNameColumn;
        private BrightIdeasSoftware.OLVColumn olvFilePathColumn;
        private BrightIdeasSoftware.OLVColumn olvVisibleColumn;
    }
}