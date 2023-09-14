namespace AtariMapMaker
{
    partial class AtariColorPicker
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelOldCol = new System.Windows.Forms.Label();
            this.labelNewCol = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(167, 72);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(78, 128);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // labelOldCol
            // 
            this.labelOldCol.Location = new System.Drawing.Point(164, 56);
            this.labelOldCol.Name = "labelOldCol";
            this.labelOldCol.Size = new System.Drawing.Size(81, 13);
            this.labelOldCol.TabIndex = 2;
            this.labelOldCol.Text = "$00 - 0";
            this.labelOldCol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNewCol
            // 
            this.labelNewCol.Location = new System.Drawing.Point(164, 203);
            this.labelNewCol.Name = "labelNewCol";
            this.labelNewCol.Size = new System.Drawing.Size(81, 13);
            this.labelNewCol.TabIndex = 3;
            this.labelNewCol.Text = "$00 - 0";
            this.labelNewCol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(144, 272);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            // 
            // AtariColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 278);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelOldCol);
            this.Controls.Add(this.labelNewCol);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AtariColorPicker";
            this.Text = "AtariColorPicker";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AtariColorPicker_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelOldCol;
        private System.Windows.Forms.Label labelNewCol;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}