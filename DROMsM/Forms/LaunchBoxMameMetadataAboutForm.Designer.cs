namespace DROMsM.Forms
{
    partial class LaunchBoxMameMetadataAboutForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(651, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "When importing a full MAME set in LaunchBox, you are presented with a window that" +
    " allows you to skip importing certain types of games.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DROMsM.Properties.Resources.LaunchBoxImportFullMAMEFullSet;
            this.pictureBox1.Location = new System.Drawing.Point(12, 108);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(780, 580);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(449, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "But specifically what games are skipped in these categories?  That\'s what this to" +
    "ol shows you.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 719);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(604, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "The way this works is by reading and parsing a file called Mame.xml that LaunchBo" +
    "x keeps in LaunchBox\\Metadata\\Mame.xml";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 743);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(675, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "This file contains all the necessary information needed for us to determine which" +
    " games LaunchBox is skipping when picking these categories.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(694, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Using this tool, you can see exactly which MAME game is from a specific region, o" +
    "r if it\'s a clone, or whether it\'s a mahjong or a casino game, etc...";
            // 
            // LaunchBoxMameMetadataAboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 780);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LaunchBoxMameMetadataAboutForm";
            this.Text = "LaunchBox MAME Metadata - What is this?";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}