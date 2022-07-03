
namespace DROMsM.Forms
{
    partial class CreateMAMEIniFilesForm
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
            this.totalRomsSelectedLabel = new System.Windows.Forms.Label();
            this.createINIFilesButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.overwriteExistingIniFilesOption = new System.Windows.Forms.CheckBox();
            this.contentForIniFiles = new System.Windows.Forms.RichTextBox();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // totalRomsSelectedLabel
            // 
            this.totalRomsSelectedLabel.AutoSize = true;
            this.totalRomsSelectedLabel.Location = new System.Drawing.Point(12, 9);
            this.totalRomsSelectedLabel.Name = "totalRomsSelectedLabel";
            this.totalRomsSelectedLabel.Size = new System.Drawing.Size(110, 13);
            this.totalRomsSelectedLabel.TabIndex = 0;
            this.totalRomsSelectedLabel.Text = "10 total roms selected";
            // 
            // createINIFilesButton
            // 
            this.createINIFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createINIFilesButton.Location = new System.Drawing.Point(637, 480);
            this.createINIFilesButton.Name = "createINIFilesButton";
            this.createINIFilesButton.Size = new System.Drawing.Size(117, 23);
            this.createINIFilesButton.TabIndex = 2;
            this.createINIFilesButton.Text = "Create ini files";
            this.createINIFilesButton.UseVisualStyleBackColor = true;
            this.createINIFilesButton.Click += new System.EventHandler(this.createINIFilesButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Text to write to each file:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(12, 36);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 52);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Options";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.overwriteExistingIniFilesOption, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(232, 18);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Overwrite existing ini files";
            // 
            // overwriteExistingIniFilesOption
            // 
            this.overwriteExistingIniFilesOption.AutoSize = true;
            this.overwriteExistingIniFilesOption.Location = new System.Drawing.Point(203, 3);
            this.overwriteExistingIniFilesOption.Name = "overwriteExistingIniFilesOption";
            this.overwriteExistingIniFilesOption.Size = new System.Drawing.Size(15, 14);
            this.overwriteExistingIniFilesOption.TabIndex = 21;
            this.overwriteExistingIniFilesOption.UseVisualStyleBackColor = true;
            // 
            // contentForIniFiles
            // 
            this.contentForIniFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentForIniFiles.Location = new System.Drawing.Point(15, 122);
            this.contentForIniFiles.Name = "contentForIniFiles";
            this.contentForIniFiles.Size = new System.Drawing.Size(739, 352);
            this.contentForIniFiles.TabIndex = 6;
            this.contentForIniFiles.Text = "";
            // 
            // CreateMAMEIniFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 515);
            this.Controls.Add(this.contentForIniFiles);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.createINIFilesButton);
            this.Controls.Add(this.totalRomsSelectedLabel);
            this.Name = "CreateMAMEIniFilesForm";
            this.Text = "Create MAME ini files";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CreateMAMEIniFilesForm_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label totalRomsSelectedLabel;
        private System.Windows.Forms.Button createINIFilesButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox overwriteExistingIniFilesOption;
        private System.Windows.Forms.RichTextBox contentForIniFiles;
    }
}