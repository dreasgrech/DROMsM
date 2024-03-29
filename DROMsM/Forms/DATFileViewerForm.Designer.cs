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
            this.olvManufacturerColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvStatusColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvEmulationColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvSaveStatesColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvPlayersColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvCoinsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenTypeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenWidth = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenHeight = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenOrientationColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvScreenRefreshRateColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvIsCloneColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvBIOSColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvMechanicalColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvRequireCHDsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvSamplesColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvDeviceColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvJoystickWays = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvControlsColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.buildLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.selectedSetsLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.visibleSetsLabel = new System.Windows.Forms.Label();
            this.totalSetsLabel = new System.Windows.Forms.Label();
            this.topMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.showAllColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideUnusedColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.resetFilteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createMAMEIniFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvDatFileListView)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.splitContainer1.IsSplitterFixed = true;
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
            this.splitContainer1.Panel2.Controls.Add(this.buildLabel);
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(2255, 969);
            this.splitContainer1.SplitterDistance = 905;
            this.splitContainer1.TabIndex = 2;
            // 
            // olvDatFileListView
            // 
            this.olvDatFileListView.AllColumns.Add(this.olvSetColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvNameColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvYearColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvManufacturerColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvStatusColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvEmulationColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvSaveStatesColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvPlayersColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvCoinsColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenTypeColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenWidth);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenHeight);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenOrientationColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvScreenRefreshRateColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvIsCloneColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvBIOSColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvMechanicalColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvRequireCHDsColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvSamplesColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvDeviceColumn);
            this.olvDatFileListView.AllColumns.Add(this.olvJoystickWays);
            this.olvDatFileListView.AllColumns.Add(this.olvControlsColumn);
            this.olvDatFileListView.AllowColumnReorder = true;
            this.olvDatFileListView.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
            this.olvDatFileListView.CellEditUseWholeCell = false;
            this.olvDatFileListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvSetColumn,
            this.olvNameColumn,
            this.olvYearColumn,
            this.olvManufacturerColumn,
            this.olvStatusColumn,
            this.olvEmulationColumn,
            this.olvSaveStatesColumn,
            this.olvPlayersColumn,
            this.olvCoinsColumn,
            this.olvScreenTypeColumn,
            this.olvScreenWidth,
            this.olvScreenHeight,
            this.olvScreenOrientationColumn,
            this.olvScreenRefreshRateColumn,
            this.olvIsCloneColumn,
            this.olvBIOSColumn,
            this.olvMechanicalColumn,
            this.olvRequireCHDsColumn,
            this.olvSamplesColumn,
            this.olvDeviceColumn,
            this.olvJoystickWays,
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
            this.olvDatFileListView.Size = new System.Drawing.Size(2255, 905);
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
            this.olvDatFileListView.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(this.olvDatFileListView_ItemsChanged);
            this.olvDatFileListView.SelectionChanged += new System.EventHandler(this.olvDatFileListView_SelectionChanged);
            // 
            // olvSetColumn
            // 
            this.olvSetColumn.AspectName = "";
            this.olvSetColumn.MinimumWidth = 60;
            this.olvSetColumn.Text = "Set";
            this.olvSetColumn.UseFiltering = false;
            this.olvSetColumn.Width = 80;
            // 
            // olvNameColumn
            // 
            this.olvNameColumn.AspectName = "";
            this.olvNameColumn.MinimumWidth = 341;
            this.olvNameColumn.Text = "Name";
            this.olvNameColumn.UseFiltering = false;
            this.olvNameColumn.Width = 341;
            // 
            // olvYearColumn
            // 
            this.olvYearColumn.AspectName = "";
            this.olvYearColumn.MaximumWidth = 70;
            this.olvYearColumn.MinimumWidth = 70;
            this.olvYearColumn.Text = "Year";
            this.olvYearColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvYearColumn.Width = 70;
            // 
            // olvManufacturerColumn
            // 
            this.olvManufacturerColumn.AspectName = "";
            this.olvManufacturerColumn.MinimumWidth = 80;
            this.olvManufacturerColumn.Text = "Manufacturer";
            this.olvManufacturerColumn.Width = 150;
            // 
            // olvStatusColumn
            // 
            this.olvStatusColumn.AspectName = "";
            this.olvStatusColumn.MinimumWidth = 80;
            this.olvStatusColumn.Text = "Status";
            this.olvStatusColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvStatusColumn.Width = 80;
            // 
            // olvEmulationColumn
            // 
            this.olvEmulationColumn.AspectName = "";
            this.olvEmulationColumn.MinimumWidth = 90;
            this.olvEmulationColumn.Text = "Emulation";
            this.olvEmulationColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvEmulationColumn.Width = 90;
            // 
            // olvSaveStatesColumn
            // 
            this.olvSaveStatesColumn.AspectName = "";
            this.olvSaveStatesColumn.MinimumWidth = 100;
            this.olvSaveStatesColumn.Text = "Save States";
            this.olvSaveStatesColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvSaveStatesColumn.Width = 100;
            // 
            // olvPlayersColumn
            // 
            this.olvPlayersColumn.AspectName = "";
            this.olvPlayersColumn.MinimumWidth = 80;
            this.olvPlayersColumn.Text = "Players";
            this.olvPlayersColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvPlayersColumn.Width = 80;
            // 
            // olvCoinsColumn
            // 
            this.olvCoinsColumn.AspectName = "";
            this.olvCoinsColumn.MinimumWidth = 80;
            this.olvCoinsColumn.Text = "Coins";
            this.olvCoinsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvCoinsColumn.Width = 80;
            // 
            // olvScreenTypeColumn
            // 
            this.olvScreenTypeColumn.AspectName = "";
            this.olvScreenTypeColumn.MinimumWidth = 80;
            this.olvScreenTypeColumn.Text = "Screen";
            this.olvScreenTypeColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvScreenTypeColumn.Width = 80;
            // 
            // olvScreenWidth
            // 
            this.olvScreenWidth.AspectName = "";
            this.olvScreenWidth.MinimumWidth = 80;
            this.olvScreenWidth.Text = "Width";
            this.olvScreenWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvScreenWidth.Width = 80;
            // 
            // olvScreenHeight
            // 
            this.olvScreenHeight.AspectName = "";
            this.olvScreenHeight.MinimumWidth = 80;
            this.olvScreenHeight.Text = "Height";
            this.olvScreenHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvScreenHeight.Width = 80;
            // 
            // olvScreenOrientationColumn
            // 
            this.olvScreenOrientationColumn.AspectName = "";
            this.olvScreenOrientationColumn.MinimumWidth = 100;
            this.olvScreenOrientationColumn.Text = "Orientation";
            this.olvScreenOrientationColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvScreenOrientationColumn.Width = 100;
            // 
            // olvScreenRefreshRateColumn
            // 
            this.olvScreenRefreshRateColumn.AspectName = "";
            this.olvScreenRefreshRateColumn.MinimumWidth = 110;
            this.olvScreenRefreshRateColumn.Text = "Refresh Rate";
            this.olvScreenRefreshRateColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvScreenRefreshRateColumn.Width = 110;
            // 
            // olvIsCloneColumn
            // 
            this.olvIsCloneColumn.AspectName = "";
            this.olvIsCloneColumn.MaximumWidth = 70;
            this.olvIsCloneColumn.MinimumWidth = 70;
            this.olvIsCloneColumn.Text = "Clone";
            this.olvIsCloneColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvIsCloneColumn.Width = 70;
            // 
            // olvBIOSColumn
            // 
            this.olvBIOSColumn.AspectName = "";
            this.olvBIOSColumn.MaximumWidth = 70;
            this.olvBIOSColumn.MinimumWidth = 70;
            this.olvBIOSColumn.Text = "BIOS";
            this.olvBIOSColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvBIOSColumn.Width = 70;
            // 
            // olvMechanicalColumn
            // 
            this.olvMechanicalColumn.AspectName = "";
            this.olvMechanicalColumn.MinimumWidth = 90;
            this.olvMechanicalColumn.Text = "Mechanical";
            this.olvMechanicalColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvMechanicalColumn.Width = 90;
            // 
            // olvRequireCHDsColumn
            // 
            this.olvRequireCHDsColumn.AspectName = "";
            this.olvRequireCHDsColumn.MaximumWidth = 70;
            this.olvRequireCHDsColumn.MinimumWidth = 70;
            this.olvRequireCHDsColumn.Text = "CHDs";
            this.olvRequireCHDsColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvRequireCHDsColumn.Width = 70;
            // 
            // olvSamplesColumn
            // 
            this.olvSamplesColumn.AspectName = "";
            this.olvSamplesColumn.MaximumWidth = 80;
            this.olvSamplesColumn.MinimumWidth = 80;
            this.olvSamplesColumn.Text = "Samples";
            this.olvSamplesColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvSamplesColumn.Width = 80;
            // 
            // olvDeviceColumn
            // 
            this.olvDeviceColumn.AspectName = "";
            this.olvDeviceColumn.MaximumWidth = 70;
            this.olvDeviceColumn.MinimumWidth = 70;
            this.olvDeviceColumn.Text = "Device";
            this.olvDeviceColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvDeviceColumn.Width = 70;
            // 
            // olvJoystickWays
            // 
            this.olvJoystickWays.AspectName = "";
            this.olvJoystickWays.MinimumWidth = 100;
            this.olvJoystickWays.Text = "Joystick Ways";
            this.olvJoystickWays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvJoystickWays.Width = 100;
            // 
            // olvControlsColumn
            // 
            this.olvControlsColumn.AspectName = "";
            this.olvControlsColumn.FillsFreeSpace = true;
            this.olvControlsColumn.MinimumWidth = 50;
            this.olvControlsColumn.Text = "Controls";
            this.olvControlsColumn.Width = 201;
            // 
            // buildLabel
            // 
            this.buildLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buildLabel.Location = new System.Drawing.Point(1968, 43);
            this.buildLabel.Name = "buildLabel";
            this.buildLabel.Size = new System.Drawing.Size(284, 13);
            this.buildLabel.TabIndex = 3;
            this.buildLabel.Text = "0.241 (mame0241)";
            this.buildLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel2.Controls.Add(this.selectedSetsLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.visibleSetsLabel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.totalSetsLabel, 1, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(165, 57);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // selectedSetsLabel
            // 
            this.selectedSetsLabel.AutoSize = true;
            this.selectedSetsLabel.Location = new System.Drawing.Point(58, 0);
            this.selectedSetsLabel.Name = "selectedSetsLabel";
            this.selectedSetsLabel.Size = new System.Drawing.Size(13, 13);
            this.selectedSetsLabel.TabIndex = 3;
            this.selectedSetsLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selected:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Visible:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total:";
            // 
            // visibleSetsLabel
            // 
            this.visibleSetsLabel.AutoSize = true;
            this.visibleSetsLabel.Location = new System.Drawing.Point(58, 20);
            this.visibleSetsLabel.Name = "visibleSetsLabel";
            this.visibleSetsLabel.Size = new System.Drawing.Size(13, 13);
            this.visibleSetsLabel.TabIndex = 5;
            this.visibleSetsLabel.Text = "0";
            // 
            // totalSetsLabel
            // 
            this.totalSetsLabel.AutoSize = true;
            this.totalSetsLabel.Location = new System.Drawing.Point(58, 40);
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
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.topMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.topMenuStrip.Name = "topMenuStrip";
            this.topMenuStrip.Size = new System.Drawing.Size(2279, 24);
            this.topMenuStrip.TabIndex = 3;
            this.topMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
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
            this.openToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(160, 6);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exportToolStripMenuItem.Text = "Export to CSV...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.toolStripSeparator3,
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(143, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.selectNoneToolStripMenuItem.Text = "Deselect All";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showColorsToolStripMenuItem,
            this.showGridLinesToolStripMenuItem,
            this.toolStripSeparator4,
            this.showAllColumnsToolStripMenuItem,
            this.hideUnusedColumnsToolStripMenuItem,
            this.toolStripSeparator5,
            this.resetFilteringToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showColorsToolStripMenuItem
            // 
            this.showColorsToolStripMenuItem.CheckOnClick = true;
            this.showColorsToolStripMenuItem.Name = "showColorsToolStripMenuItem";
            this.showColorsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.showColorsToolStripMenuItem.Text = "Show MAME Status colors";
            this.showColorsToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showColorsToolStripMenuItem_CheckedChanged);
            // 
            // showGridLinesToolStripMenuItem
            // 
            this.showGridLinesToolStripMenuItem.CheckOnClick = true;
            this.showGridLinesToolStripMenuItem.Name = "showGridLinesToolStripMenuItem";
            this.showGridLinesToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.showGridLinesToolStripMenuItem.Text = "Show grid lines";
            this.showGridLinesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.showGridLinesToolStripMenuItem_CheckedChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(209, 6);
            // 
            // showAllColumnsToolStripMenuItem
            // 
            this.showAllColumnsToolStripMenuItem.Name = "showAllColumnsToolStripMenuItem";
            this.showAllColumnsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.showAllColumnsToolStripMenuItem.Text = "Show all columns";
            this.showAllColumnsToolStripMenuItem.Click += new System.EventHandler(this.showAllColumnsToolStripMenuItem_Click);
            // 
            // hideUnusedColumnsToolStripMenuItem
            // 
            this.hideUnusedColumnsToolStripMenuItem.Name = "hideUnusedColumnsToolStripMenuItem";
            this.hideUnusedColumnsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.hideUnusedColumnsToolStripMenuItem.Text = "Hide unused columns";
            this.hideUnusedColumnsToolStripMenuItem.Click += new System.EventHandler(this.hideUnusedColumnsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(209, 6);
            // 
            // resetFilteringToolStripMenuItem
            // 
            this.resetFilteringToolStripMenuItem.Name = "resetFilteringToolStripMenuItem";
            this.resetFilteringToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.resetFilteringToolStripMenuItem.Text = "Reset column filters";
            this.resetFilteringToolStripMenuItem.Click += new System.EventHandler(this.resetFilteringToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createMAMEIniFilesToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // createMAMEIniFilesToolStripMenuItem
            // 
            this.createMAMEIniFilesToolStripMenuItem.Name = "createMAMEIniFilesToolStripMenuItem";
            this.createMAMEIniFilesToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.createMAMEIniFilesToolStripMenuItem.Text = "Create MAME ini files...";
            this.createMAMEIniFilesToolStripMenuItem.Click += new System.EventHandler(this.createMAMEIniFilesToolStripMenuItem_Click);
            // 
            // DATFileViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2279, 1030);
            this.Controls.Add(this.datFilePathLabel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.topMenuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.topMenuStrip;
            this.MinimizeBox = false;
            this.Name = "DATFileViewerForm";
            this.ShowInTaskbar = false;
            this.Text = "DATFileViewerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DATFileViewerForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvDatFileListView)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.Label buildLabel;
        private System.Windows.Forms.Label totalSetsLabel;
        private System.Windows.Forms.Label label2;
        private BrightIdeasSoftware.FastObjectListView olvDatFileListView;
        private BrightIdeasSoftware.OLVColumn olvSetColumn;
        private BrightIdeasSoftware.OLVColumn olvNameColumn;
        private BrightIdeasSoftware.OLVColumn olvYearColumn;
        private BrightIdeasSoftware.OLVColumn olvManufacturerColumn;
        private BrightIdeasSoftware.OLVColumn olvStatusColumn;
        private BrightIdeasSoftware.OLVColumn olvEmulationColumn;
        private BrightIdeasSoftware.OLVColumn olvSaveStatesColumn;
        private BrightIdeasSoftware.OLVColumn olvPlayersColumn;
        private BrightIdeasSoftware.OLVColumn olvCoinsColumn;
        private BrightIdeasSoftware.OLVColumn olvControlsColumn;
        private BrightIdeasSoftware.OLVColumn olvScreenTypeColumn;
        private BrightIdeasSoftware.OLVColumn olvScreenOrientationColumn;
        private BrightIdeasSoftware.OLVColumn olvScreenRefreshRateColumn;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showColorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showGridLinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvIsCloneColumn;
        private BrightIdeasSoftware.OLVColumn olvBIOSColumn;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvMechanicalColumn;
        private BrightIdeasSoftware.OLVColumn olvRequireCHDsColumn;
        private BrightIdeasSoftware.OLVColumn olvSamplesColumn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetFilteringToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvDeviceColumn;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideUnusedColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem createMAMEIniFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label selectedSetsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label visibleSetsLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvScreenWidth;
        private BrightIdeasSoftware.OLVColumn olvScreenHeight;
        private BrightIdeasSoftware.OLVColumn olvJoystickWays;
    }
}