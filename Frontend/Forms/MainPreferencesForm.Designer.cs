namespace Frontend
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
            this.allowedSimilarityValueTextbox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.matchUsingGamelistXMLNameCheckbox = new System.Windows.Forms.CheckBox();
            this.autoExpandAfterOperationsCheckbox = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 93);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.allowedSimilarityValueTextbox, 1, 0);
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
            // allowedSimilarityValueTextbox
            // 
            this.allowedSimilarityValueTextbox.Location = new System.Drawing.Point(183, 0);
            this.allowedSimilarityValueTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.allowedSimilarityValueTextbox.Name = "allowedSimilarityValueTextbox";
            this.allowedSimilarityValueTextbox.Size = new System.Drawing.Size(82, 20);
            this.allowedSimilarityValueTextbox.TabIndex = 2;
            this.allowedSimilarityValueTextbox.Leave += new System.EventHandler(this.allowedSimilarityValueTextbox_Leave);
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
            // MainPreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 115);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "MainPreferencesForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainPreferencesForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainPreferencesForm_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox allowedSimilarityValueTextbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox matchUsingGamelistXMLNameCheckbox;
        private System.Windows.Forms.CheckBox autoExpandAfterOperationsCheckbox;
        private System.Windows.Forms.Label label10;
    }
}