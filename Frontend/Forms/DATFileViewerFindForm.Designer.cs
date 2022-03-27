namespace DROMsM.Forms
{
    partial class DATFileViewerFindForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.searchTermTextBox = new System.Windows.Forms.TextBox();
            this.findButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.useRegularExpressionsCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            // 
            // searchTermTextBox
            // 
            this.searchTermTextBox.Location = new System.Drawing.Point(75, 10);
            this.searchTermTextBox.Name = "searchTermTextBox";
            this.searchTermTextBox.Size = new System.Drawing.Size(224, 20);
            this.searchTermTextBox.TabIndex = 1;
            this.searchTermTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTermTextBox_KeyDown);
            // 
            // findButton
            // 
            this.findButton.Location = new System.Drawing.Point(305, 8);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(75, 23);
            this.findButton.TabIndex = 2;
            this.findButton.Text = "Find";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.findButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.useRegularExpressionsCheckbox);
            this.groupBox1.Location = new System.Drawing.Point(16, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 45);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Find options";
            // 
            // useRegularExpressionsCheckbox
            // 
            this.useRegularExpressionsCheckbox.AutoSize = true;
            this.useRegularExpressionsCheckbox.Location = new System.Drawing.Point(6, 19);
            this.useRegularExpressionsCheckbox.Name = "useRegularExpressionsCheckbox";
            this.useRegularExpressionsCheckbox.Size = new System.Drawing.Size(144, 17);
            this.useRegularExpressionsCheckbox.TabIndex = 0;
            this.useRegularExpressionsCheckbox.Text = "Use Regular Expressions";
            this.useRegularExpressionsCheckbox.UseVisualStyleBackColor = true;
            // 
            // DATFileViewerFindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 111);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.findButton);
            this.Controls.Add(this.searchTermTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DATFileViewerFindForm";
            this.ShowInTaskbar = false;
            this.Text = "Search in DAT file";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DATFileViewerFindForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DATFileViewerFindForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox searchTermTextBox;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox useRegularExpressionsCheckbox;
    }
}