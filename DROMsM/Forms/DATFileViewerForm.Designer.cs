﻿namespace DROMsM.Forms
{
    partial class DATFileViewerForm
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
            this.datFilePathLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.olvDatFileListView = new BrightIdeasSoftware.FastObjectListView();
            this.olvSetColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvYearColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvManufacturer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvStatusColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvEmulationColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvSaveStates = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvPlayersColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCoinsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenOrientation = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenRefreshRate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvIsCloneColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvBIOSColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvControlsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buildLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.totalSetsLabel = new System.Windows.Forms.Label();
            this.topMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvDatFileListView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.topMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // datFilePathLabel
            // 
            this.datFilePathLabel.AutoSize = true;
            this.datFilePathLabel.Location = new System.Drawing.Point(12, 33);
            this.datFilePathLabel.Name = "datFilePathLabel";
            this.datFilePathLabel.Size = new System.Drawing.Size(115, 13);
            this.datFilePathLabel.TabIndex = 0;
            this.datFilePathLabel.Text = "C:\\My directory\\file.dat";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(12, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.olvDatFileListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1692, 969);
            this.splitContainer1.SplitterDistance = 926;
            this.splitContainer1.TabIndex = 2;
            // 
            // olvDatFileListView
            // 
            this.olvDatFileListView.AllColumns.Add(this.olvSetColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvNameColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvYearColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvManufacturer);
            this.olvDatFileListView.AllColumns.Add(this.olvStatusColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvEmulationColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvSaveStates);
            this.olvDatFileListView.AllColumns.Add(this.olvPlayersColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvCoinsColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenType);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenOrientation);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenRefreshRate);
            this.olvDatFileListView.AllColumns.Add(this.olvIsCloneColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvBIOSColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvControlsColumn);
            this.olvDatFileListView.AllowColumnReorder = true;
            this.olvDatFileListView.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
            this.olvDatFileListView.CellEditUseWholeCell = false;
            this.olvDatFileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvSetColumn,
            this.olvNameColumn,
            this.olvYearColumn,
            this.olvManufacturer,
            this.olvStatusColumn,
            this.olvEmulationColumn,
            this.olvSaveStates,
            this.olvPlayersColumn,
            this.olvCoinsColumn,
            this.olvScreenType,
            this.olvScreenOrientation,
            this.olvScreenRefreshRate,
            this.olvIsCloneColumn,
            this.olvBIOSColumn,
            this.olvControlsColumn});
            this.olvDatFileListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvDatFileListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvDatFileListView.FullRowSelect = true;
            this.olvDatFileListView.GridLines = true;
            this.olvDatFileListView.HideSelection = false;
            this.olvDatFileListView.Location = new System.Drawing.Point(0, 0);
            this.olvDatFileListView.Margin = new System.Windows.Forms.Padding(0);
            this.olvDatFileListView.Name = "olvDatFileListView";
            this.olvDatFileListView.ShowGroups = false;
            this.olvDatFileListView.Size = new System.Drawing.Size(1692, 926);
            this.olvDatFileListView.TabIndex = 4;
            this.olvDatFileListView.TintSortColumn = true;
            this.olvDatFileListView.UseAlternatingBackColors = true;
            this.olvDatFileListView.UseCompatibleStateImageBehavior = false;
            this.olvDatFileListView.UseFilterIndicator = true;
            this.olvDatFileListView.UseFiltering = true;
            this.olvDatFileListView.UseHotControls = false;
            this.olvDatFileListView.View = System.Windows.Forms.View.Details;
            this.olvDatFileListView.VirtualMode = true;
            this.olvDatFileListView.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.olvDatFileListView_FormatRow);
            // 
            // olvSetColumn
            // 
            this.olvSetColumn.AspectName = "Name";
            this.olvSetColumn.MinimumWidth = 30;
            this.olvSetColumn.Text = "Set";
            this.olvSetColumn.UseFiltering = false;
            // 
            // olvNameColumn
            // 
            this.olvNameColumn.AspectName = "Description";
            this.olvNameColumn.MinimumWidth = 341;
            this.olvNameColumn.Text = "Name";
            this.olvNameColumn.UseFiltering = false;
            this.olvNameColumn.Width = 341;
            // 
            // olvYearColumn
            // 
            this.olvYearColumn.AspectName = "Year";
            this.olvYearColumn.DisplayIndex = 3;
            this.olvYearColumn.MaximumWidth = 40;
            this.olvYearColumn.MinimumWidth = 40;
            this.olvYearColumn.Text = "Year";
            this.olvYearColumn.Width = 40;
            // 
            // olvManufacturer
            // 
            this.olvManufacturer.AspectName = "Manufacturer";
            this.olvManufacturer.DisplayIndex = 4;
            this.olvManufacturer.MinimumWidth = 80;
            this.olvManufacturer.Text = "Manufacturer";
            this.olvManufacturer.Width = 150;
            // 
            // olvStatusColumn
            // 
            this.olvStatusColumn.AspectName = "Status";
            this.olvStatusColumn.DisplayIndex = 5;
            this.olvStatusColumn.MinimumWidth = 50;
            this.olvStatusColumn.Text = "Status";
            this.olvStatusColumn.Width = 50;
            // 
            // olvEmulationColumn
            // 
            this.olvEmulationColumn.AspectName = "Emulation";
            this.olvEmulationColumn.DisplayIndex = 6;
            this.olvEmulationColumn.MinimumWidth = 60;
            this.olvEmulationColumn.Text = "Emulation";
            // 
            // olvSaveStates
            // 
            this.olvSaveStates.AspectName = "SaveStates";
            this.olvSaveStates.DisplayIndex = 7;
            this.olvSaveStates.MinimumWidth = 70;
            this.olvSaveStates.Text = "Save States";
            this.olvSaveStates.Width = 70;
            // 
            // olvPlayersColumn
            // 
            this.olvPlayersColumn.AspectName = "Players";
            this.olvPlayersColumn.DisplayIndex = 8;
            this.olvPlayersColumn.MaximumWidth = 50;
            this.olvPlayersColumn.MinimumWidth = 50;
            this.olvPlayersColumn.Text = "Players";
            this.olvPlayersColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvPlayersColumn.Width = 50;
            // 
            // olvCoinsColumn
            // 
            this.olvCoinsColumn.AspectName = "Coins";
            this.olvCoinsColumn.DisplayIndex = 9;
            this.olvCoinsColumn.MaximumWidth = 40;
            this.olvCoinsColumn.MinimumWidth = 40;
            this.olvCoinsColumn.Text = "Coins";
            this.olvCoinsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvCoinsColumn.Width = 40;
            // 
            // olvScreenType
            // 
            this.olvScreenType.AspectName = "ScreenType";
            this.olvScreenType.DisplayIndex = 10;
            this.olvScreenType.MinimumWidth = 50;
            this.olvScreenType.Text = "Screen";
            this.olvScreenType.Width = 50;
            // 
            // olvScreenOrientation
            // 
            this.olvScreenOrientation.AspectName = "ScreenOrientation";
            this.olvScreenOrientation.DisplayIndex = 11;
            this.olvScreenOrientation.MinimumWidth = 70;
            this.olvScreenOrientation.Text = "Orientation";
            this.olvScreenOrientation.Width = 70;
            // 
            // olvScreenRefreshRate
            // 
            this.olvScreenRefreshRate.AspectName = "ScreenRefreshRate";
            this.olvScreenRefreshRate.DisplayIndex = 12;
            this.olvScreenRefreshRate.MinimumWidth = 80;
            this.olvScreenRefreshRate.Text = "Refresh Rate";
            this.olvScreenRefreshRate.Width = 80;
            // 
            // olvIsCloneColumn
            // 
            this.olvIsCloneColumn.AspectName = "IsClone";
            this.olvIsCloneColumn.DisplayIndex = 2;
            this.olvIsCloneColumn.MaximumWidth = 40;
            this.olvIsCloneColumn.MinimumWidth = 40;
            this.olvIsCloneColumn.Text = "Clone";
            this.olvIsCloneColumn.Width = 40;
            // 
            // olvBIOSColumn
            // 
            this.olvBIOSColumn.AspectName = "IsBIOS";
            this.olvBIOSColumn.MaximumWidth = 40;
            this.olvBIOSColumn.MinimumWidth = 40;
            this.olvBIOSColumn.Text = "BIOS";
            this.olvBIOSColumn.Width = 40;
            // 
            // olvControlsColumn
            // 
            this.olvControlsColumn.AspectName = "Controls";
            this.olvControlsColumn.FillsFreeSpace = true;
            this.olvControlsColumn.MinimumWidth = 50;
            this.olvControlsColumn.Text = "Controls";
            this.olvControlsColumn.Width = 201;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.00422F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.99578F));
            this.tableLayoutPanel1.Controls.Add(this.buildLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.totalSetsLabel, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 42);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buildLabel
            // 
            this.buildLabel.AutoSize = true;
            this.buildLabel.Location = new System.Drawing.Point(67, 0);
            this.buildLabel.Name = "buildLabel";
            this.buildLabel.Size = new System.Drawing.Size(95, 13);
            this.buildLabel.TabIndex = 3;
            this.buildLabel.Text = "0.241 (mame0241)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Build";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total sets";
            // 
            // totalSetsLabel
            // 
            this.totalSetsLabel.AutoSize = true;
            this.totalSetsLabel.Location = new System.Drawing.Point(67, 20);
            this.totalSetsLabel.Name = "totalSetsLabel";
            this.totalSetsLabel.Size = new System.Drawing.Size(13, 13);
            this.totalSetsLabel.TabIndex = 1;
            this.totalSetsLabel.Text = "0";
            // 
            // topMenuStrip
            // 
            this.topMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.topMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.topMenuStrip.Name = "topMenuStrip";
            this.topMenuStrip.Size = new System.Drawing.Size(1716, 24);
            this.topMenuStrip.TabIndex = 3;
            this.topMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.findToolStripMenuItem.Text = "Find...";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showColorsToolStripMenuItem,
            this.showGridLinesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // showColorsToolStripMenuItem
            // 
            this.showColorsToolStripMenuItem.CheckOnClick = true;
            this.showColorsToolStripMenuItem.Name = "showColorsToolStripMenuItem";
            this.showColorsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.showColorsToolStripMenuItem.Text = "Show Colors";
            this.showColorsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showColorsToolStripMenuItem_CheckedChanged);
            // 
            // showGridLinesToolStripMenuItem
            // 
            this.showGridLinesToolStripMenuItem.CheckOnClick = true;
            this.showGridLinesToolStripMenuItem.Name = "showGridLinesToolStripMenuItem";
            this.showGridLinesToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.showGridLinesToolStripMenuItem.Text = "Show Grid Lines";
            this.showGridLinesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showGridLinesToolStripMenuItem_CheckedChanged);
            // 
            // DATFileViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1716, 1030);
            this.Controls.Add(this.datFilePathLabel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.topMenuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.topMenuStrip;
            this.Name = "DATFileViewerForm";
            this.ShowInTaskbar = false;
            this.Text = "DATFileViewerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DATFileViewerForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvDatFileListView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.topMenuStrip.ResumeLayout(false);
            this.topMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label datFilePathLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MenuStrip topMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label buildLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label totalSetsLabel;
        private System.Windows.Forms.Label label2;
        private BrightIdeasSoftware.FastObjectListView olvDatFileListView;
        private BrightIdeasSoftware.OLVColumn olvSetColumn;
        private BrightIdeasSoftware.OLVColumn olvNameColumn;
        private BrightIdeasSoftware.OLVColumn olvYearColumn;
        private BrightIdeasSoftware.OLVColumn olvManufacturer;
        private BrightIdeasSoftware.OLVColumn olvStatusColumn;
        private BrightIdeasSoftware.OLVColumn olvEmulationColumn;
        private BrightIdeasSoftware.OLVColumn olvSaveStates;
        private BrightIdeasSoftware.OLVColumn olvPlayersColumn;
        private BrightIdeasSoftware.OLVColumn olvCoinsColumn;
        private BrightIdeasSoftware.OLVColumn olvControlsColumn;
        private BrightIdeasSoftware.OLVColumn olvScreenType;
        private BrightIdeasSoftware.OLVColumn olvScreenOrientation;
        private BrightIdeasSoftware.OLVColumn olvScreenRefreshRate;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showColorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvIsCloneColumn;
        private BrightIdeasSoftware.OLVColumn olvBIOSColumn;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    }
}