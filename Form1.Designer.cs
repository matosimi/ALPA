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
            pictureBoxSource = new PictureBox();
            button1 = new Button();
            pictureBoxAprox = new PictureBox();
            pictureBoxAtariAprox = new PictureBox();
            pictureBoxPalette = new PictureBox();
            pictureBoxAproxInverse = new PictureBox();
            pictureBoxAtariAproxInverse = new PictureBox();
            pictureBoxAtariMix = new PictureBox();
            button2 = new Button();
            pictureBoxCharMask = new PictureBox();
            checkBoxSimpleAvg = new CheckBox();
            comboBoxDither = new ComboBox();
            checkBoxUseDither = new CheckBox();
            comboBoxDistance = new ComboBox();
            label1 = new Label();
            buttonOpen = new Button();
            openFileDialog1 = new OpenFileDialog();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAprox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAprox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPalette).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAproxInverse).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAproxInverse).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariMix).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharMask).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.Image = (Image)resources.GetObject("pictureBoxSource.Image");
            pictureBoxSource.Location = new Point(12, 12);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Size = new Size(128, 192);
            pictureBoxSource.TabIndex = 0;
            pictureBoxSource.TabStop = false;
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
            // pictureBoxAprox
            // 
            pictureBoxAprox.Location = new Point(3, 3);
            pictureBoxAprox.Name = "pictureBoxAprox";
            pictureBoxAprox.Size = new Size(256, 136);
            pictureBoxAprox.TabIndex = 2;
            pictureBoxAprox.TabStop = false;
            // 
            // pictureBoxAtariAprox
            // 
            pictureBoxAtariAprox.Location = new Point(3, 145);
            pictureBoxAtariAprox.Name = "pictureBoxAtariAprox";
            pictureBoxAtariAprox.Size = new Size(256, 136);
            pictureBoxAtariAprox.TabIndex = 3;
            pictureBoxAtariAprox.TabStop = false;
            // 
            // pictureBoxPalette
            // 
            pictureBoxPalette.Location = new Point(12, 240);
            pictureBoxPalette.Name = "pictureBoxPalette";
            pictureBoxPalette.Size = new Size(256, 192);
            pictureBoxPalette.TabIndex = 4;
            pictureBoxPalette.TabStop = false;
            // 
            // pictureBoxAproxInverse
            // 
            pictureBoxAproxInverse.Location = new Point(265, 3);
            pictureBoxAproxInverse.Name = "pictureBoxAproxInverse";
            pictureBoxAproxInverse.Size = new Size(256, 136);
            pictureBoxAproxInverse.TabIndex = 6;
            pictureBoxAproxInverse.TabStop = false;
            // 
            // pictureBoxAtariAproxInverse
            // 
            pictureBoxAtariAproxInverse.Location = new Point(265, 145);
            pictureBoxAtariAproxInverse.Name = "pictureBoxAtariAproxInverse";
            pictureBoxAtariAproxInverse.Size = new Size(256, 136);
            pictureBoxAtariAproxInverse.TabIndex = 7;
            pictureBoxAtariAproxInverse.TabStop = false;
            // 
            // pictureBoxAtariMix
            // 
            pictureBoxAtariMix.Location = new Point(265, 287);
            pictureBoxAtariMix.Name = "pictureBoxAtariMix";
            pictureBoxAtariMix.Size = new Size(256, 136);
            pictureBoxAtariMix.TabIndex = 8;
            pictureBoxAtariMix.TabStop = false;
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
            // pictureBoxCharMask
            // 
            pictureBoxCharMask.Location = new Point(3, 287);
            pictureBoxCharMask.Name = "pictureBoxCharMask";
            pictureBoxCharMask.Size = new Size(256, 136);
            pictureBoxCharMask.TabIndex = 10;
            pictureBoxCharMask.TabStop = false;
            // 
            // checkBoxSimpleAvg
            // 
            checkBoxSimpleAvg.AutoSize = true;
            checkBoxSimpleAvg.Checked = true;
            checkBoxSimpleAvg.CheckState = CheckState.Checked;
            checkBoxSimpleAvg.Location = new Point(171, 12);
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
            comboBoxDither.Location = new Point(171, 62);
            comboBoxDither.Name = "comboBoxDither";
            comboBoxDither.Size = new Size(96, 23);
            comboBoxDither.TabIndex = 13;
            // 
            // checkBoxUseDither
            // 
            checkBoxUseDither.AutoSize = true;
            checkBoxUseDither.Checked = true;
            checkBoxUseDither.CheckState = CheckState.Checked;
            checkBoxUseDither.Location = new Point(171, 37);
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
            comboBoxDistance.Location = new Point(171, 124);
            comboBoxDistance.Name = "comboBoxDistance";
            comboBoxDistance.Size = new Size(96, 23);
            comboBoxDistance.TabIndex = 15;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(167, 98);
            label1.Name = "label1";
            label1.Size = new Size(100, 15);
            label1.TabIndex = 16;
            label1.Text = "Distance method:";
            // 
            // buttonOpen
            // 
            buttonOpen.Location = new Point(12, 210);
            buttonOpen.Name = "buttonOpen";
            buttonOpen.Size = new Size(75, 23);
            buttonOpen.TabIndex = 17;
            buttonOpen.Text = "Open";
            buttonOpen.UseVisualStyleBackColor = true;
            buttonOpen.Click += buttonOpen_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(pictureBoxAprox);
            flowLayoutPanel1.Controls.Add(pictureBoxAproxInverse);
            flowLayoutPanel1.Controls.Add(pictureBoxAtariAprox);
            flowLayoutPanel1.Controls.Add(pictureBoxAtariAproxInverse);
            flowLayoutPanel1.Controls.Add(pictureBoxCharMask);
            flowLayoutPanel1.Controls.Add(pictureBoxAtariMix);
            flowLayoutPanel1.Dock = DockStyle.Right;
            flowLayoutPanel1.Location = new Point(383, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(536, 536);
            flowLayoutPanel1.TabIndex = 18;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(919, 536);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(buttonOpen);
            Controls.Add(label1);
            Controls.Add(comboBoxDistance);
            Controls.Add(checkBoxUseDither);
            Controls.Add(comboBoxDither);
            Controls.Add(checkBoxSimpleAvg);
            Controls.Add(button2);
            Controls.Add(pictureBoxPalette);
            Controls.Add(button1);
            Controls.Add(pictureBoxSource);
            Name = "Form1";
            Text = "AlterLinePictureAproximator";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAprox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAprox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPalette).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAproxInverse).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariAproxInverse).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAtariMix).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharMask).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxSource;
        private Button button1;
        private PictureBox pictureBoxAprox;
        private PictureBox pictureBoxAtariAprox;
        private PictureBox pictureBoxPalette;
        private PictureBox pictureBoxAproxInverse;
        private PictureBox pictureBoxAtariAproxInverse;
        private PictureBox pictureBoxAtariMix;
        private Button button2;
        private PictureBox pictureBoxCharMask;
        private CheckBox checkBoxSimpleAvg;
        private ComboBox comboBoxDither;
        private CheckBox checkBoxUseDither;
        private ComboBox comboBoxDistance;
        private Label label1;
        private Button buttonOpen;
        private OpenFileDialog openFileDialog1;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}