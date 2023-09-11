namespace AlterLinePictureAproximator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBox1 = new PictureBox();
            button1 = new Button();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            checkBoxInverse = new CheckBox();
            pictureBox5 = new PictureBox();
            pictureBox6 = new PictureBox();
            pictureBox7 = new PictureBox();
            button2 = new Button();
            pictureBox8 = new PictureBox();
            checkBoxSimpleAvg = new CheckBox();
            comboBoxDither = new ComboBox();
            checkBoxUseDither = new CheckBox();
            comboBoxDistance = new ComboBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(160, 160);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(311, 12);
            button1.Name = "button1";
            button1.Size = new Size(53, 64);
            button1.TabIndex = 1;
            button1.Text = "Aprioximate";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(370, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(256, 136);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Location = new Point(370, 154);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(256, 136);
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(12, 240);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(256, 200);
            pictureBox4.TabIndex = 4;
            pictureBox4.TabStop = false;
            // 
            // checkBoxInverse
            // 
            checkBoxInverse.AutoSize = true;
            checkBoxInverse.Location = new Point(178, 12);
            checkBoxInverse.Name = "checkBoxInverse";
            checkBoxInverse.Size = new Size(63, 19);
            checkBoxInverse.TabIndex = 5;
            checkBoxInverse.Text = "inverse";
            checkBoxInverse.UseVisualStyleBackColor = true;
            checkBoxInverse.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // pictureBox5
            // 
            pictureBox5.Location = new Point(632, 12);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(256, 136);
            pictureBox5.TabIndex = 6;
            pictureBox5.TabStop = false;
            // 
            // pictureBox6
            // 
            pictureBox6.Location = new Point(632, 154);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(256, 136);
            pictureBox6.TabIndex = 7;
            pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            pictureBox7.Location = new Point(632, 296);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(256, 136);
            pictureBox7.TabIndex = 8;
            pictureBox7.TabStop = false;
            // 
            // button2
            // 
            button2.Location = new Point(311, 84);
            button2.Name = "button2";
            button2.Size = new Size(53, 64);
            button2.TabIndex = 9;
            button2.Text = "Mix";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // pictureBox8
            // 
            pictureBox8.Location = new Point(370, 296);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(256, 136);
            pictureBox8.TabIndex = 10;
            pictureBox8.TabStop = false;
            // 
            // checkBoxSimpleAvg
            // 
            checkBoxSimpleAvg.AutoSize = true;
            checkBoxSimpleAvg.Checked = true;
            checkBoxSimpleAvg.CheckState = CheckState.Checked;
            checkBoxSimpleAvg.Location = new Point(178, 37);
            checkBoxSimpleAvg.Name = "checkBoxSimpleAvg";
            checkBoxSimpleAvg.Size = new Size(117, 19);
            checkBoxSimpleAvg.TabIndex = 11;
            checkBoxSimpleAvg.Text = "Simple averaging";
            checkBoxSimpleAvg.UseVisualStyleBackColor = true;
            checkBoxSimpleAvg.CheckedChanged += checkBoxSimple_CheckedChanged;
            // 
            // comboBoxDither
            // 
            comboBoxDither.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDither.FormattingEnabled = true;
            comboBoxDither.Items.AddRange(new object[] { "chess", "sierra", "F-S" });
            comboBoxDither.Location = new Point(176, 87);
            comboBoxDither.Name = "comboBoxDither";
            comboBoxDither.Size = new Size(96, 23);
            comboBoxDither.TabIndex = 13;
            // 
            // checkBoxUseDither
            // 
            checkBoxUseDither.AutoSize = true;
            checkBoxUseDither.Checked = true;
            checkBoxUseDither.CheckState = CheckState.Checked;
            checkBoxUseDither.Location = new Point(176, 62);
            checkBoxUseDither.Name = "checkBoxUseDither";
            checkBoxUseDither.Size = new Size(96, 19);
            checkBoxUseDither.TabIndex = 14;
            checkBoxUseDither.Text = "Use dithering";
            checkBoxUseDither.UseVisualStyleBackColor = true;
            checkBoxUseDither.CheckedChanged += checkBoxUseDither_CheckedChanged;
            // 
            // comboBoxDistance
            // 
            comboBoxDistance.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDistance.FormattingEnabled = true;
            comboBoxDistance.Items.AddRange(new object[] { "difference", "RGB euclid", "YUV euclid" });
            comboBoxDistance.Location = new Point(176, 149);
            comboBoxDistance.Name = "comboBoxDistance";
            comboBoxDistance.Size = new Size(96, 23);
            comboBoxDistance.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(172, 123);
            label1.Name = "label1";
            label1.Size = new Size(100, 15);
            label1.TabIndex = 16;
            label1.Text = "Distance method:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 536);
            Controls.Add(label1);
            Controls.Add(comboBoxDistance);
            Controls.Add(checkBoxUseDither);
            Controls.Add(comboBoxDither);
            Controls.Add(checkBoxSimpleAvg);
            Controls.Add(pictureBox8);
            Controls.Add(button2);
            Controls.Add(pictureBox7);
            Controls.Add(pictureBox6);
            Controls.Add(pictureBox5);
            Controls.Add(checkBoxInverse);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "AlterLinePictureAproximator";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private CheckBox checkBoxInverse;
        private PictureBox pictureBox5;
        private PictureBox pictureBox6;
        private PictureBox pictureBox7;
        private Button button2;
        private PictureBox pictureBox8;
        private CheckBox checkBoxSimpleAvg;
        private ComboBox comboBoxDither;
        private CheckBox checkBoxUseDither;
        private ComboBox comboBoxDistance;
        private Label label1;
    }
}