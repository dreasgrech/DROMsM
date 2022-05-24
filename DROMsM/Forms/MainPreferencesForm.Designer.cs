namespace DROMsM.Forms
{
    partial class MainPreferencesForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.allowedSimilarityValueTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.matchUsingGamelistXMLNameCheckbox = new System.Windows.Forms.CheckBox();
            this.autoExpandAfterOperationsCheckbox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.datFileViewer_OnlyShowUsedColumns = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.launchBox_BasePathTextBox = new System.Windows.Forms.TextBox();
            this.launchBox_BasePath_BrowseButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 93);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operations";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.allowedSimilarityValueTextBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.matchUsingGamelistXMLNameCheckbox, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.autoExpandAfterOperationsCheckbox, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(366, 59);
            this.tableLayoutPanel4.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 20);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Match using gamelist.xml name";
            // 
            // allowedSimilarityValueTextBox
            // 
            this.allowedSimilarityValueTextBox.Location = new System.Drawing.Point(183, 0);
            this.allowedSimilarityValueTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.allowedSimilarityValueTextBox.Name = "allowedSimilarityValueTextBox";
            this.allowedSimilarityValueTextBox.Size = new System.Drawing.Size(82, 20);
            this.allowedSimilarityValueTextBox.TabIndex = 2;
            this.allowedSimilarityValueTextBox.Leave += new System.EventHandler(this.allowedSimilarityValueTextBox_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Allowed Similarity Value";
            // 
            // matchUsingGamelistXMLNameCheckbox
            // 
            this.matchUsingGamelistXMLNameCheckbox.AutoSize = true;
            this.matchUsingGamelistXMLNameCheckbox.Location = new System.Drawing.Point(186, 23);
            this.matchUsingGamelistXMLNameCheckbox.Name = "matchUsingGamelistXMLNameCheckbox";
            this.matchUsingGamelistXMLNameCheckbox.Size = new System.Drawing.Size(15, 14);
            this.matchUsingGamelistXMLNameCheckbox.TabIndex = 21;
            this.matchUsingGamelistXMLNameCheckbox.UseVisualStyleBackColor = true;
            // 
            // autoExpandAfterOperationsCheckbox
            // 
            this.autoExpandAfterOperationsCheckbox.AutoSize = true;
            this.autoExpandAfterOperationsCheckbox.Location = new System.Drawing.Point(186, 43);
            this.autoExpandAfterOperationsCheckbox.Name = "autoExpandAfterOperationsCheckbox";
            this.autoExpandAfterOperationsCheckbox.Size = new System.Drawing.Size(15, 14);
            this.autoExpandAfterOperationsCheckbox.TabIndex = 23;
            this.autoExpandAfterOperationsCheckbox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 40);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(143, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Auto-expand after operations";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Location = new System.Drawing.Point(12, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(650, 55);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DAT File Viewer";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.02208F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.97792F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.datFileViewer_OnlyShowUsedColumns, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(634, 21);
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
            this.label1.Text = "Only show used columns";
            // 
            // datFileViewer_OnlyShowUsedColumns
            // 
            this.datFileViewer_OnlyShowUsedColumns.AutoSize = true;
            this.datFileViewer_OnlyShowUsedColumns.Location = new System.Drawing.Point(186, 3);
            this.datFileViewer_OnlyShowUsedColumns.Name = "datFileViewer_OnlyShowUsedColumns";
            this.datFileViewer_OnlyShowUsedColumns.Size = new System.Drawing.Size(15, 14);
            this.datFileViewer_OnlyShowUsedColumns.TabIndex = 21;
            this.datFileViewer_OnlyShowUsedColumns.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel2);
            this.groupBox3.Location = new System.Drawing.Point(12, 197);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(650, 63);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "LaunchBox";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.39117F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.60883F));
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(634, 38);
            this.tableLayoutPanel2.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Location";
            // 
            // launchBox_BasePathTextBox
            // 
            this.launchBox_BasePathTextBox.Location = new System.Drawing.Point(5, 5);
            this.launchBox_BasePathTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.launchBox_BasePathTextBox.Name = "launchBox_BasePathTextBox";
            this.launchBox_BasePathTextBox.Size = new System.Drawing.Size(354, 20);
            this.launchBox_BasePathTextBox.TabIndex = 21;
            this.launchBox_BasePathTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.launchBox_BasePathTextBox_MouseClick);
            // 
            // launchBox_BasePath_BrowseButton
            // 
            this.launchBox_BasePath_BrowseButton.Location = new System.Drawing.Point(367, 3);
            this.launchBox_BasePath_BrowseButton.Name = "launchBox_BasePath_BrowseButton";
            this.launchBox_BasePath_BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.launchBox_BasePath_BrowseButton.TabIndex = 22;
            this.launchBox_BasePath_BrowseButton.Text = "Browse";
            this.launchBox_BasePath_BrowseButton.UseVisualStyleBackColor = true;
            this.launchBox_BasePath_BrowseButton.Click += new System.EventHandler(this.launchBox_BasePath_BrowseButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.launchBox_BasePathTextBox);
            this.flowLayoutPanel1.Controls.Add(this.launchBox_BasePath_BrowseButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(182, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(449, 32);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // MainPreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 276);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainPreferencesForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainPreferencesForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainPreferencesForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox allowedSimilarityValueTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox matchUsingGamelistXMLNameCheckbox;
        private System.Windows.Forms.CheckBox autoExpandAfterOperationsCheckbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox datFileViewer_OnlyShowUsedColumns;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox launchBox_BasePathTextBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button launchBox_BasePath_BrowseButton;
    }
}